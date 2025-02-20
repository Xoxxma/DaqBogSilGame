using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // Ну, это наш потолок здоровья, выше не прыгнешь
    private int currentHealth; // А вот это — то, сколько у нас сейчас ХП

    private void Start()
    {
        currentHealth = maxHealth; // Как только стартуем, у нас полный бак здоровья
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Если уже в отключке — смысла в уроне нет

        currentHealth -= damage; // Срезаем здоровье на указанное число
        if (currentHealth <= 0) // Если вдруг стало 0 или ниже...
        {
            Die(); // ...значит, пора на покой
        }
    }

    public void Heal(int amount)
    {
        // Подлечиваемся, но не выходим за пределы макс. ХП
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        // Просто выводим в консоль, что персонаж откинулся
        Debug.Log($"{gameObject.name} умер!");
    }
}
