using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�� ������ ��������!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}