using UnityEngine;
using UnityEngine.UI;

public class OpenURLButton : MonoBehaviour
{
    [SerializeField] private string url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"; //ссылка на рофлянчи
    [SerializeField] private Button button; // Ссылка на UI-кнопку, которую можно задать в инспекторе

    private void Start() 
    {
        if (button == null) // Если кнопка не была назначена в инспекторе, пробуем получить по автомату
        {
            button = GetComponent<Button>();
        }
        // Если кнопка найдена, добавляем то что будет реагировать на нажатие кнопки
        if (button != null)
        {
            button.onClick.AddListener(OpenURL);// При нажатии вызываем метод OpenURL
        }
    }

    private void OpenURL()
    {
        Application.OpenURL(url);// Открывает ссылку в браузере , ну юрл мы в начале поставили что за ссылка
    }
}
