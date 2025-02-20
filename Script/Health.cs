using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth = 100; // ��, ��� ��� ������� ��������, ���� �� ��������
    private int currentHealth; // � ��� ��� � ��, ������� � ��� ������ ��

    private void Start()
    {
        currentHealth = maxHealth; // ��� ������ ��������, � ��� ������ ��� ��������
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // ���� ��� � �������� � ������ � ����� ���

        currentHealth -= damage; // ������� �������� �� ��������� �����
        if (currentHealth <= 0) // ���� ����� ����� 0 ��� ����...
        {
            Die(); // ...������, ���� �� �����
        }
    }

    public void Heal(int amount)
    {
        // �������������, �� �� ������� �� ������� ����. ��
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        // ������ ������� � �������, ��� �������� ���������
        Debug.Log($"{gameObject.name} ����!");
    }
}
