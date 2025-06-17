using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirJugadorMinimapa : MonoBehaviour
{
    public Transform jugador;

    void LateUpdate()
    {
        Vector3 nuevaPos = jugador.position;
        nuevaPos.y = transform.position.y; // mantener altura fija
        transform.position = nuevaPos;
    }
}
