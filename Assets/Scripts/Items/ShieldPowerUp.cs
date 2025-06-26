    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entró tiene el PlayerShield
        PlayerShield playerShield = other.GetComponent<PlayerShield>();

        if (playerShield != null)
        {
            playerShield.ActivateShield();
            Destroy(gameObject); // Elimina la cápsula del juego
        }
    }
}
