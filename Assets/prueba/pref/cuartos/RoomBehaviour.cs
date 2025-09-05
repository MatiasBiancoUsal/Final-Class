using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public GameObject[] doors;
    public GameObject hatch; // Escotilla en el prefab

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
        }
    }

    public void EnableHatch()
    {
        if (hatch != null)
            hatch.SetActive(true);
    }

    public void DisableHatch()
    {
        if (hatch != null)
            hatch.SetActive(false);
    }
}
