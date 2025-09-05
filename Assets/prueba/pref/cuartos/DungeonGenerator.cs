using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    [System.Serializable]
    public class Rule
    {
        public GameObject room;
        public bool obligatory;
    }

    public Vector2Int size;
    public int startPos = 0;
    public Rule[] rooms;
    public Vector2 offset;
    private GameObject player; // 👈 el jugador ya está en la escena

    List<Cell> board;

    void Start()
    {
       
        MazeGenerator();
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }

        int currentCell = startPos;
        Stack<int> path = new Stack<int>();
        int k = 0;

        while (k < 1000)
        {
            k++;
            board[currentCell].visited = true;

            if (currentCell == board.Count - 1) break;

            List<int> neighbors = CheckNeighbors(currentCell);

            if (neighbors.Count == 0)
            {
                if (path.Count == 0) break;
                currentCell = path.Pop();
            }
            else
            {
                path.Push(currentCell);
                int newCell = neighbors[Random.Range(0, neighbors.Count)];

                if (newCell > currentCell)
                {
                    if (newCell - 1 == currentCell)
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if (newCell + 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                }
            }
        }

        GenerateDungeon();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();

        if (cell - size.x >= 0 && !board[cell - size.x].visited)
            neighbors.Add(cell - size.x);
        if (cell + size.x < board.Count && !board[cell + size.x].visited)
            neighbors.Add(cell + size.x);
        if ((cell + 1) % size.x != 0 && !board[cell + 1].visited)
            neighbors.Add(cell + 1);
        if (cell % size.x != 0 && !board[cell - 1].visited)
            neighbors.Add(cell - 1);

        return neighbors;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    void GenerateDungeon()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No se encontró ningún objeto con el tag 'Player' en la escena.");
            return;
        }

        List<Vector2Int> visitedCells = new List<Vector2Int>();
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                if (board[i + j * size.x].visited)
                    visitedCells.Add(new Vector2Int(i, j));
            }
        }

        Shuffle(visitedCells);

        List<Rule> obligatoryRooms = new List<Rule>();
        List<Rule> optionalRooms = new List<Rule>();

        foreach (var rule in rooms)
        {
            if (rule.obligatory) obligatoryRooms.Add(rule);
            else optionalRooms.Add(rule);
        }

        if (obligatoryRooms.Count > visitedCells.Count)
        {
            Debug.LogError("No hay suficientes celdas para los cuartos obligatorios.");
            return;
        }

        int cellIndex = 0;
        HashSet<GameObject> usedRooms = new HashSet<GameObject>();
        RoomBehaviour firstRoom = null;
        RoomBehaviour lastRoom = null;

        // 1. Colocar los cuartos obligatorios
        foreach (var room in obligatoryRooms)
        {
            var pos = visitedCells[cellIndex++];
            usedRooms.Add(room.room);

            var newRoom = Instantiate(
                room.room,
                new Vector3(pos.x * offset.x, 0, -pos.y * offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>();

            newRoom.UpdateRoom(board[pos.x + pos.y * size.x].status);
            newRoom.name += $" {pos.x}-{pos.y}";
            newRoom.DisableHatch();

            if (firstRoom == null)
                firstRoom = newRoom;

            lastRoom = newRoom;
        }

        // 2. Colocar opcionales
        Shuffle(optionalRooms);

        while (cellIndex < visitedCells.Count)
        {
            var pos = visitedCells[cellIndex++];

            Rule selectedRoom = null;

            foreach (var room in optionalRooms)
            {
                if (!usedRooms.Contains(room.room))
                {
                    selectedRoom = room;
                    usedRooms.Add(room.room);
                    break;
                }
            }

            if (selectedRoom == null)
            {
                Debug.Log("No quedan cuartos opcionales únicos, usando el primero repetido.");
                selectedRoom = optionalRooms[0];
            }

            var newRoom = Instantiate(
                selectedRoom.room,
                new Vector3(pos.x * offset.x, 0, -pos.y * offset.y),
                Quaternion.identity,
                transform
            ).GetComponent<RoomBehaviour>();

            newRoom.UpdateRoom(board[pos.x + pos.y * size.x].status);
            newRoom.name += $" {pos.x}-{pos.y}";
            newRoom.DisableHatch();

            if (firstRoom == null)
                firstRoom = newRoom;

            lastRoom = newRoom;
        }

        // 3. Activar la escotilla SOLO en el último cuarto
        if (lastRoom != null)
        {
            lastRoom.EnableHatch();
        }

        // 4. Mover jugador al primer cuarto
        if (firstRoom != null )
        {
            player.transform.position = firstRoom.transform.position + new Vector3(0, 1, 0);
        }
    }
}