using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Метод для загрузки игровой сцены
    public void StartGame()
    {
        SceneManager.LoadScene("Game"); // Загружаем сцену с игрой
    }
}
