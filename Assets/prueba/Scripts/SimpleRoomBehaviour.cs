using UnityEngine;

public class SimpleRoomBehaviour : MonoBehaviour
{
    public GameObject[] doors; // up, down, right, left
    public Vector2Int gridPosition;

    public void SetPosition(Vector2Int pos)
    {
        gridPosition = pos;
    }

    public void UpdateDoors(bool[] active)
    {
        for (int i = 0; i < 4; i++)
        {
            if (doors != null && i < doors.Length && doors[i] != null)
            {
                doors[i].SetActive(active[i]);
            }
        }
    }
}
