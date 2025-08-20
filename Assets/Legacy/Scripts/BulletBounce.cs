using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBounce : MonoBehaviour
{
    public float lifetime = 5f; // Tiempo de vida m�ximo
    private bool yaReboto = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Ignorar colisiones con el jugador o m�s proyectiles
        if (collision.collider.CompareTag("Player") || collision.collider.CompareTag("Bala")) return;

        if (!yaReboto)
        {
            yaReboto = true;
        }
        else
        {
            // Desactivar rebote o destruir bala despu�s del segundo impacto
            Destroy(gameObject);
        }
    }
}
