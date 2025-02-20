using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f; // �������� ����� ������
    public float maxDistance = 10f; // ������������ ���������, ������� ������ ����� ���������
    public int damage = 20; // ����, ��������� �������

    private Vector3 startPosition; // �����, ������ ������ ���� ��������
    private Rigidbody2D rb; // ���������� ���� ������

    void Start()
    {
        startPosition = transform.position; // ���������� ��������� �������
        rb = GetComponent<Rigidbody2D>(); // �������� ��������� Rigidbody2D

        Destroy(gameObject, 3f); // ���� �� 3 ������� �� �������� �� �� ��� � ����������
    }

    void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance) // ���� ������ ������� ������� ������
        {
            Destroy(gameObject); // ������� �
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // ����� ������������ � ���-��
    {
        if (collision.gameObject.CompareTag("Enemy")) // ���������, ������ �� �� �����
        {
            Health enemyHealth = collision.GetComponent<Health>(); // �������� ��������� �������� �����
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // ������� ����
            }
            Destroy(gameObject); // ���������� ������ ����� ���������
        }
    }
}
