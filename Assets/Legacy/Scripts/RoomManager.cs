using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public int numeroDeEnemigos;
    public bool cuartoCompletado = false;

    private void Update()
    {
        if (numeroDeEnemigos <= 0)
        {
            cuartoCompletado = true;
        }
    }

}
