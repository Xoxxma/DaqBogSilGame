using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; } // ���, ��������, ����� ����� ����� ���� ����������� �� ������ ������

    private int _maxHealth; // ���, ��� ������ ������������ ��������
    private int _currentHealth; // ���, ��� ������� ��������

    public int MaxHealth // ������-������ ��� ����. ��������
    {
        get => _maxHealth; // ���, ������ ���������� ��������
        private set => _maxHealth = value; // ������������� ����� �������� (�� ������ ������ ������)
    }

    public int CurrentHealth // ���������� ��� �������� ��������
    {
        get => _currentHealth; // ���, �������� ������� ��
        private set => _currentHealth = value; // ������������� (�� �� ��� ���, � ������ ���� �����)
    }

    private void Awake()
    {
        // ���, ��� ���������, ���� �� ��� ������� ����� ������
        if (Instance == null)
        {
            Instance = this; // ���� ���, �� ��������� ���� �������
        }
        else
        {
            Destroy(gameObject); // ���� ��� ����, ������� ���� ������, ������ ��� �� ������
            return;
        }
    }

    public void SetMaxHealth(int value)
    {
        // ���, ������������� ������������ ��������
        MaxHealth = value;
        CurrentHealth = MaxHealth; // � ����� �� ��������� ������ ������
    }

    public void TakeDamage(int damage)
    {
        // ���, �������� ��, �� �� ��� ���� � ����� (��� ����� �� �����)
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
    }

    public void Heal(int amount)
    {
        // ���, ������ ������, �� �� ���� ���������
        CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
    }
}
