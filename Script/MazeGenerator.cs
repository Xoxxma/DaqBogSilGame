using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int width = 20;      // ������ ���������
    public int height = 20;     // ������ ���������
    public GameObject wallPrefab;   // ������ �����
    public GameObject floorPrefab;  // ������ ����
    public GameObject exitPrefab;   // ������ ������

    private int[,] maze; // ������ ��� �������� ���������

    void Start()
    {
        GenerateMaze();
    }

    // ��������� ���������
    void GenerateMaze()
    {
        maze = new int[width, height];   // ������ ������ ��������

        // ������������� ���� ������ ��� ���� (0 - �����, 1 - ����)
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = 0;  // ��������� �� �������
            }
        }

        // �������� � ����� (1,1) � ������ ��� ����� ���������
        GeneratePath(1, 1);

        // ���������� �������� � �����
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x - width / 2, y - height / 2, 0);

                // �����
                if (maze[x, y] == 0)
                {
                    Instantiate(wallPrefab, pos, Quaternion.identity);
                }
                // ����
                else if (maze[x, y] == 1)
                {
                    Instantiate(floorPrefab, pos, Quaternion.identity);
                }
            }
        }

        // ������������� ����� � ���������
        Vector3 exitPos = new Vector3(width - 2 - width / 2, height - 2 - height / 2, 0); // ����� �� ����������� (width-2, height-2)
        Instantiate(exitPrefab, exitPos, Quaternion.identity);
    }

    // ����������� �������� ��� ��������� ����� � ����������� ������� 5 ������
    void GeneratePath(int x, int y)
    {
        maze[x, y] = 1; // ������������� ����

        // ������ � 4 ���������� ������������� (������, �����, ����, �����)
        int[] directions = { 0, 1, 2, 3 };
        System.Random rand = new System.Random();
        directions = directions.OrderBy(n => rand.Next()).ToArray(); // ������������ �����������

        foreach (int dir in directions)
        {
            int nx = x, ny = y;
            if (dir == 0) nx += 5; // ������, ������� 5 ������
            if (dir == 1) nx -= 5; // �����, ������� 5 ������
            if (dir == 2) ny += 5; // ����, ������� 5 ������
            if (dir == 3) ny -= 5; // �����, ������� 5 ������

            // ���������, ��� �� ������� �� ������� ��������� � ��� ������ ��� �� �������
            if (nx > 0 && ny > 0 && nx < width - 1 && ny < height - 1 && maze[nx, ny] == 0)
            {
                maze[nx, ny] = 1; // ��������� ������
                maze[(x + nx) / 2, (y + ny) / 2] = 1; // ��������� ������ ���������� (����������� ����� ������)
                GeneratePath(nx, ny); // ���������� ���������� ����
            }
        }
    }
}
