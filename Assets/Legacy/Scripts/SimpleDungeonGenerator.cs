using UnityEngine;
using System.Collections.Generic;

public class SimpleDungeonGenerator : MonoBehaviour
{
    public Vector2Int size = new Vector2Int(5, 5);
    public GameObject[] roomPrefabs; // Cada cuarto debe ser único
    public Vector2 roomOffset = new Vector2(10, 10);

    private SimpleRoomBehaviour[,] grid;

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        grid = new SimpleRoomBehaviour[size.x, size.y];

        // Mezclar los prefabs únicos
        List<GameObject> shuffledRooms = new List<GameObject>(roomPrefabs);
        Shuffle(shuffledRooms);

        int placed = 0;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                if (placed >= shuffledRooms.Count)
                    return; // No repetir cuartos, detener cuando se acaben

                Vector3 position = new Vector3(x * roomOffset.x, 0, -y * roomOffset.y);
                GameObject roomObj = Instantiate(shuffledRooms[placed++], position, Quaternion.identity, transform);
                SimpleRoomBehaviour room = roomObj.GetComponent<SimpleRoomBehaviour>();

                grid[x, y] = room;
                room.SetPosition(new Vector2Int(x, y));
            }
        }

        // Activar puertas hacia cuartos vecinos
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                SimpleRoomBehaviour room = grid[x, y];
                if (room == null) continue;

                bool[] doors = new bool[4]; // up, down, right, left

                // Revisa vecinos
                if (IsRoomAt(x, y + 1)) doors[0] = true;
                if (IsRoomAt(x, y - 1)) doors[1] = true;
                if (IsRoomAt(x + 1, y)) doors[2] = true;
                if (IsRoomAt(x - 1, y)) doors[3] = true;

                room.UpdateDoors(doors);
            }
        }
    }

    bool IsRoomAt(int x, int y)
    {
        return x >= 0 && y >= 0 && x < size.x && y < size.y && grid[x, y] != null;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}

