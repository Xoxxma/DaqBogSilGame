using UnityEngine;
using UnityEngine.UI;

public class OpenURLButton : MonoBehaviour
{
    [SerializeField] private string url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"; //������ �� ��������
    [SerializeField] private Button button; // ������ �� UI-������, ������� ����� ������ � ����������

    private void Start() 
    {
        if (button == null) // ���� ������ �� ���� ��������� � ����������, ������� �������� �� ��������
        {
            button = GetComponent<Button>();
        }
        // ���� ������ �������, ��������� �� ��� ����� ����������� �� ������� ������
        if (button != null)
        {
            button.onClick.AddListener(OpenURL);// ��� ������� �������� ����� OpenURL
        }
    }

    private void OpenURL()
    {
        Application.OpenURL(url);// ��������� ������ � �������� , �� ��� �� � ������ ��������� ��� �� ������
    }
}
