using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int precio = 100;
    public GameObject efectoPowerUp;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlayerDinero dineroJugador = other.GetComponent<PlayerDinero>();
            if (dineroJugador != null && dineroJugador.dinero >= precio)
            {
                dineroJugador.RestarDinero(precio);
                ActivarPowerUp(other.gameObject); // << Activar efecto
                Debug.Log("PowerUp comprado por $" + precio);
                Destroy(gameObject); // Elimina el objeto PowerUp de la escena
            }
            else
            {
                Debug.Log("No tenés suficiente dinero");
            }
        }
    }

    void ActivarPowerUp(GameObject jugador)
    {
        if (efectoPowerUp != null)
            Instantiate(efectoPowerUp, jugador.transform.position, Quaternion.identity);
    }
}





