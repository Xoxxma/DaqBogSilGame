using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; } // Йоу, синглтон, чтобы везде можно было достучаться до статов игрока

    private int _maxHealth; // Тип, тут храним максимальное здоровье
    private int _currentHealth; // Йоу, тут текущее здоровье

    public int MaxHealth // Геттер-сеттер для макс. здоровья
    {
        get => _maxHealth; // Тип, просто возвращаем значение
        private set => _maxHealth = value; // Устанавливаем новое значение (но только внутри класса)
    }

    public int CurrentHealth // Аналогично для текущего здоровья
    {
        get => _currentHealth; // Йоу, забираем текущее ХП
        private set => _currentHealth = value; // Устанавливаем (но не абы кто, а только этот класс)
    }

    private void Awake()
    {
        // Йоу, тут проверяем, есть ли уже инстанс этого класса
        if (Instance == null)
        {
            Instance = this; // Если нет, то назначаем себя главным
        }
        else
        {
            Destroy(gameObject); // Если уже есть, удаляем этот объект, потому что он лишний
            return;
        }
    }

    public void SetMaxHealth(int value)
    {
        // Тип, устанавливаем максимальное здоровье
        MaxHealth = value;
        CurrentHealth = MaxHealth; // И сразу же полностью хиллим игрока
    }

    public void TakeDamage(int damage)
    {
        // Йоу, отнимаем ХП, но не даём уйти в минус (ибо зомби не нужны)
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
    }

    public void Heal(int amount)
    {
        // Тип, хиллим игрока, но не выше максимума
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
    }
}
