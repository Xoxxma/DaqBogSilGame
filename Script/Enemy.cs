using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public int maxHP = 100; // Верхняя планка здоровья врага
    private int currentHP; // Текущее хп
    private bool isDead = false; // Если true — враг уже не жилец

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;

    public HeroController player; // Наш герой, которого враг хочет сожрать
    public float aggroRange = 5f; // Дистанция, с которой враг начинает агриться
    public float attackRange = 1.5f; // Дальность атаки
    public float speed = 2f; // Скорость передвижения
    public int attackDamage = 20; // Урон, который он наносит
    private bool isAttacking = false; // Атакует ли враг сейчас?
    private float attackCooldown = 1.5f; // Перезарядка атаки
    private bool useFirstAttack = true; // Переключение между двумя атаками

    public Transform attackPoint; // Точка, откуда враг атакует
    public Vector2 attackSize = new Vector2(2f, 1f); // Размер удара

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        currentHP = maxHP; // Стартуем с полным здоровьем

        if (player == null)
        {
            player = FindObjectOfType<HeroController>(); // Если игрок не задан вручную, ищем его в сцене
        }
    }

    private void Update()
    {
        if (isDead || isAttacking || player == null) return; // Если мертв или атакует — ничего не делаем

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position); // Узнаём, насколько далеко игрок

        if (distanceToPlayer <= attackRange) // Если враг уже в радиусе атаки
        {
            StartCoroutine(AttackPlayer()); // Начинаем лупить
        }
        else if (distanceToPlayer <= aggroRange) // Если игрок не в зоне атаки, но уже близко
        {
            ChasePlayer(); // Гонимся за ним
        }
        else
        {
            Patrol(); // Если игрок далеко, чилим
        }
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized; // Направление к игроку
        _rigidbody2D.velocity = direction * speed; // Двигаемся в его сторону
        _animator.SetBool("IsWalking", true); // Включаем анимацию ходьбы

        if (direction.x > 0)
            FlipEnemy(false); // Если игрок справа — смотрим направо
        else if (direction.x < 0)
            FlipEnemy(true); // Если слева — налево
    }

    private void Patrol()
    {
        _rigidbody2D.velocity = Vector2.zero; // Стоим на месте
        _animator.SetBool("IsWalking", false); // Останавливаем анимацию ходьбы
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        _rigidbody2D.velocity = Vector2.zero; // Останавливаемся перед атакой
        _animator.SetBool("IsWalking", false); // Вырубаем анимацию движения

        if (useFirstAttack)
        {
            _animator.SetTrigger("AttackTrigger"); // Запускаем первую атаку
        }
        else
        {
            _animator.SetTrigger("Attack02Trigger"); // Запускаем альтернативную атаку
        }
        useFirstAttack = !useFirstAttack; // Меняем атаку для следующего удара

        yield return new WaitForSeconds(0.5f); // Даем анимации проиграться перед уроном

        DealDamage(); // Применяем урон игроку

        yield return new WaitForSeconds(attackCooldown); // Ждем перед следующей атакой
        isAttacking = false; // Разрешаем снова атаковать
    }

    private void DealDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0); // Проверяем, кого задели

        foreach (Collider2D hit in hitPlayers)
        {
            if (hit.CompareTag("Player")) // Если это игрок
            {
                HeroController hero = hit.GetComponent<HeroController>(); // Берем его скрипт
                if (hero != null)
                {
                    hero.TakeDamage(attackDamage); // Наносим урон
                    Debug.Log($"🔥 Враг нанес {attackDamage} урона игроку!"); // Логируем в консоль
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Если уже мертв, игнорим

        currentHP -= damage; // Минусуем хп
        Debug.Log($"Враг получил урон: {damage}. Осталось HP: {currentHP}"); // Сообщаем в консоль

        if (currentHP > 0)
        {
            StartCoroutine(PlayTakeDamageAnimation()); // Проигрываем анимацию получения урона
        }
        else
        {
            Die(); // Ну всё, капец пришёл
        }
    }

    private IEnumerator PlayTakeDamageAnimation()
    {
        _animator.SetTrigger("TakeDamage"); // Запускаем анимацию удара
        yield return new WaitForSeconds(0.3f); // Чуть ждём
        _animator.ResetTrigger("TakeDamage"); // Сбрасываем триггер
    }

    private void Die()
    {
        if (isDead) return; // Если уже мертв, не повторяем

        isDead = true; // Фиксируем смерть
        Debug.Log("☠️ Враг умирает..."); // Сообщаем в консоль
        _animator.SetTrigger("Die"); // Включаем анимацию смерти
        _rigidbody2D.velocity = Vector2.zero; // Останавливаем движения

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; // Вырубаем коллайдер

        StartCoroutine(WaitForDeathAnimation()); // Ждём конца анимации перед удалением объекта
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length); // Ждём окончания анимации смерти
        Debug.Log("💀 Враг удален с карты"); // Сообщаем в консоль
        Destroy(gameObject); // Удаляем объект
    }

    private void FlipEnemy(bool isFlipped)
    {
        _spriteRenderer.flipX = isFlipped; // Разворачиваем спрайт

        float direction = isFlipped ? -1f : 1f;
        attackPoint.localPosition = new Vector3(direction * Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z); // Разворачиваем точку атаки вместе с врагом
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return; // Если точки атаки нет — выходим

        Gizmos.color = Color.red; // Рисуем красным
        Gizmos.DrawWireCube(attackPoint.position, attackSize); // Отображаем область атаки в редакторе
    }
}
