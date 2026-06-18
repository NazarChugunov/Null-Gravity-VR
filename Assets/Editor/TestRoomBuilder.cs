#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class TestRoomBuilder
{
    // Цей рядок створить нову кнопку в самому верхньому меню Unity!
    [MenuItem("Gravity Protocol/Створити тестову кімнату")]
    public static void BuildRoom()
    {
        GameObject room = new GameObject("ZeroG_TestRoom");
        
        float size = 20f; // Розмір кімнати
        float wallThickness = 1f;

        // Внутрішня функція для швидкого створення стін
        void MakeWall(string name, Vector3 pos, Vector3 scale)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
            wall.transform.parent = room.transform;
            wall.transform.position = pos;
            wall.transform.localScale = scale;
        }

        // Будуємо підлогу, стелю та 4 бокові стіни
        MakeWall("Floor", new Vector3(0, -size/2, 0), new Vector3(size, wallThickness, size));
        MakeWall("Ceiling", new Vector3(0, size/2, 0), new Vector3(size, wallThickness, size));
        MakeWall("Wall_1", new Vector3(0, 0, size/2), new Vector3(size, size, wallThickness));
        MakeWall("Wall_2", new Vector3(0, 0, -size/2), new Vector3(size, size, wallThickness));
        MakeWall("Wall_3", new Vector3(size/2, 0, 0), new Vector3(wallThickness, size, size));
        MakeWall("Wall_4", new Vector3(-size/2, 0, 0), new Vector3(wallThickness, size, size));

        // Спавнимо випадкові куби для візуального відчуття швидкості
        for (int i = 0; i < 20; i++)
        {
            GameObject obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obstacle.name = "FloatingCube_" + i;
            obstacle.transform.parent = room.transform;
            obstacle.transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(-8f, 8f), Random.Range(-8f, 8f));
            obstacle.transform.localScale = Vector3.one * Random.Range(0.5f, 2f);
            obstacle.transform.rotation = Random.rotation;
        }

        // Переміщаємо гравця чітко в центр кімнати
        GameObject player = GameObject.Find("XR Origin (XR Rig)");
        if (player != null) player.transform.position = Vector3.zero;

        Debug.Log("Тестова кімната для польотів успішно згенерована!");
    }
}
#endif