using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f; // Скорость полёта стрелы
    public float maxDistance = 10f; // Максимальная дистанция, которую стрела может пролететь
    public int damage = 20; // Урон, наносимый стрелой

    private Vector3 startPosition; // Точка, откуда стрела была выпущена
    private Rigidbody2D rb; // Физическое тело стрелы

    void Start()
    {
        startPosition = transform.position; // Запоминаем начальную позицию
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D

        Destroy(gameObject, 3f); // Если за 3 секунды не ударится ни во что — уничтожаем
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance) // Если стрела улетела слишком далеко
        {
            Destroy(gameObject); // Удаляем её
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // Когда сталкиваемся с чем-то
    {
        if (collision.gameObject.CompareTag("Enemy")) // Проверяем, попали ли во врага
        {
            Health enemyHealth = collision.GetComponent<Health>(); // Получаем компонент здоровья врага
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Наносим урон
            }
            Destroy(gameObject); // Уничтожаем стрелу после попадания
        }
    }
}
