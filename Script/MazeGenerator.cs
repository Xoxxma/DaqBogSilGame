using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 20;      // Ширина лабиринта
    public int height = 20;     // Высота лабиринта
    public GameObject wallPrefab;   // Префаб стены
    public GameObject floorPrefab;  // Префаб пола
    public GameObject exitPrefab;   // Префаб выхода

    private int[,] maze; // Массив для хранения лабиринта

    void Start()
    {
        GenerateMaze();
    }

    // Генерация лабиринта
    void GenerateMaze()
    {
        maze = new int[width, height];   // Создаём пустой лабиринт

        // Инициализация всех клеток как стен (0 - стена, 1 - путь)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 0;  // Заполняем всё стенами
            }
        }

        // Начинаем с точки (1,1) — обычно это старт лабиринта
        GeneratePath(1, 1);

        // Отображаем лабиринт в сцене
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x - width / 2, y - height / 2, 0);

                // Стены
                if (maze[x, y] == 0)
                {
                    Instantiate(wallPrefab, pos, Quaternion.identity);
                }
                // Путь
                else if (maze[x, y] == 1)
                {
                    Instantiate(floorPrefab, pos, Quaternion.identity);
                }
            }
        }

        // Устанавливаем выход в лабиринте
        Vector3 exitPos = new Vector3(width - 2 - width / 2, height - 2 - height / 2, 0); // выход на координатах (width-2, height-2)
        Instantiate(exitPrefab, exitPos, Quaternion.identity);
    }

    // Рекурсивный алгоритм для генерации путей с минимальной шириной 5 блоков
    void GeneratePath(int x, int y)
    {
        maze[x, y] = 1; // Устанавливаем путь

        // Массив с 4 возможными направлениями (вправо, влево, вниз, вверх)
        int[] directions = { 0, 1, 2, 3 };
        System.Random rand = new System.Random();
        directions = directions.OrderBy(n => rand.Next()).ToArray(); // Перемешиваем направления

        foreach (int dir in directions)
        {
            int nx = x, ny = y;
            if (dir == 0) nx += 5; // Вправо, минимум 5 блоков
            if (dir == 1) nx -= 5; // Влево, минимум 5 блоков
            if (dir == 2) ny += 5; // Вниз, минимум 5 блоков
            if (dir == 3) ny -= 5; // Вверх, минимум 5 блоков

            // Проверяем, что не выходим за пределы лабиринта и что клетка ещё не открыта
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[nx, ny] == 0)
            {
                maze[nx, ny] = 1; // Открываем клетку
                maze[(x + nx) / 2, (y + ny) / 2] = 1; // Открываем клетку посередине (разделитель между путями)
                GeneratePath(nx, ny); // Рекурсивно генерируем путь
            }
        }
    }
}
