using UnityEngine;
using UnityEngine.SceneManagement;
using System; // Йоу, убрали UnityEngine.UI, потому что UI не нужен

public class HeroController : MonoBehaviour
{
    public float speed; // Тип, скорость передвижения героя
    public LayerMask enemyLayer; // Слой врагов, чтобы их атаковать
    public Transform attackPoint; // Точка, из которой происходит атака
    public Vector2 attackSize = new Vector2(2f, 1f); // Размер зоны удара
    public int attackDamage = 20; // Йоу, урон, который наносим врагам
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform shootPoint; // Точка, из которой вылетают стрелы
    public float arrowSpeed = 10f; // Скорость полёта стрелы

    private Rigidbody2D _rigidbody2D; // Физика персонажа
    private Animator _animator; // Аниматор для смены анимаций
    private SpriteRenderer _spriteRenderer; // Отвечает за отрисовку спрайта
    private bool isAttacking = false; // Флаг атаки (чтобы нельзя было спамить)
    private bool isHurt = false; // Флаг получения урона
    private float attackHoldTime = 0f; // Время удержания атаки
    private bool isHoldingAttack = false; // Проверяем, держит ли игрок кнопку атаки

    public int MaxHealth { get; private set; } = 100; // Йоу, макс. хп по дефолту 100
    public int CurrentHealth { get; private set; } // Тип, текущее хп

    private void Awake()
    {
        // Йоу, на старте получаем компоненты персонажа
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentHealth = MaxHealth; // Тип, на старте у нас фулл ХП
    }

    private void Update()
    {
        Move(); // Двигаем персонажа
        HandleAttack(); // Обрабатываем атаки
    }

    private void Move()
    {
        if (isHurt || isAttacking) return; // Тип, если бьют или атакуем — не двигаемся

        float hor = Input.GetAxisRaw("Horizontal"); // Йоу, считываем горизонтальный ввод
        float ver = Input.GetAxisRaw("Vertical"); // Йоу, вертикальный ввод

        Vector2 movement = new Vector2(hor, ver).normalized; // Нормализуем, чтобы не было ускорений по диагонали
        _rigidbody2D.velocity = movement * speed; // Двигаем персонажа
        _animator.SetBool("isWalking", movement.sqrMagnitude > 0); // Включаем анимацию ходьбы

        if (hor != 0) // Тип, если идём влево или вправо, переворачиваем спрайт
        {
            _spriteRenderer.flipX = hor < 0;
            FlipAttackPoint(hor < 0); // Переворачиваем точку атаки
        }
    }

    private void HandleAttack()
    {
        if (isAttacking) return; // Если уже атакуем, не даём атаковать ещё раз

        if (Input.GetMouseButtonDown(0)) // ЛКМ нажали - начали заряжать атаку
        {
            attackHoldTime = Time.time;
            isHoldingAttack = true;
        }

        if (Input.GetMouseButtonUp(0) && isHoldingAttack) // Отпустили ЛКМ - атакуем
        {
            float heldTime = Time.time - attackHoldTime;
            StartAttack(heldTime >= 0.3f ? "attack2" : "attack"); // Тип, если держали 0.3 сек+, делаем сильную атаку
            isHoldingAttack = false;
        }

        if (Input.GetMouseButtonDown(1)) // ПКМ — комбо атака + стрельба
        {
            StartAttack("attack3");
            ShootArrow();
        }
    }

    private void EndAttack()
    {
        isAttacking = false; // Разрешаем снова атаковать
        _animator.SetBool("isAttacking", false);
    }

    private void StartAttack(string attackType)
    {
        isAttacking = true; // Флаг атаки активен
        _rigidbody2D.velocity = Vector2.zero; // Останавливаем персонажа при атаке
        _animator.SetBool("isWalking", false); // Выключаем анимацию ходьбы
        _animator.SetBool("isAttacking", true); // Включаем анимацию атаки
        _animator.SetTrigger(attackType); // Запускаем нужный тип атаки
        Invoke(nameof(DealDamage), 0.2f); // Через 0.2 сек бьём врагов
    }

    private void DealDamage()
    {
        // Йоу, проверяем, кто попал в зону атаки
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackSize, 0, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            enemyScript?.TakeDamage(attackDamage); // Тип, наносим урон, если это враг
        }
    }

    private void ShootArrow()
    {
        if (!arrowPrefab || !shootPoint) return; // Тип, если нет стрел или точки спавна - выходим

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector3 shootDirection = (mousePosition - shootPoint.position).normalized; // Йоу, получаем направление выстрела

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        if (rb)
        {
            rb.velocity = shootDirection * arrowSpeed; // Тип, задаём скорость стреле
            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle); // Поворачиваем стрелу в нужную сторону
        }
    }

    public void TakeDamage(int damage)
    {
        if (isHurt || CurrentHealth <= 0) return; // Тип, если уже биты или мертвы, не получаем урон

        CurrentHealth = Mathf.Max(0, CurrentHealth - damage); // Отнимаем ХП, но не даём уйти в минус

        isHurt = true; // Флаг получения урона
        isAttacking = false; // Прерываем атаку
        _animator.SetTrigger("Hurt"); // Запускаем анимацию ранения

        if (CurrentHealth <= 0) Die(); // Если ХП 0 - умираем
        else Invoke(nameof(EndHurt), 0.5f); // Иначе через 0.5 сек снова можем двигаться
    }

    private void EndHurt() => isHurt = false; // Тип, перестаём быть ранеными

    private void Die()
    {
        _animator.SetTrigger("Die"); // Йоу, проигрываем анимацию смерти
        _rigidbody2D.velocity = Vector2.zero; // Останавливаем персонажа
        _rigidbody2D.simulated = false; // Отключаем физику
        enabled = false; // Вырубаем скрипт
        Invoke(nameof(LoadDeathScene), 2f); // Через 2 сек грузим сцену смерти
    }

    private void LoadDeathScene() => SceneManager.LoadScene("DieHirohito"); // Загружаем сцену смерти

    private void FlipAttackPoint(bool isFlipped) => attackPoint.localPosition = new Vector3((isFlipped ? -1 : 1) * Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, attackPoint.localPosition.z);

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount); // Тип, хиллим героя, но не выше макс. ХП
    }
}
