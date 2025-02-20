using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Если в редакторе — просто стопаем игру
#else
        Application.Quit(); // Если в билде — реально закрываем приложение
#endif
    }
}
