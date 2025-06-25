using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int precio = 100;
    public GameObject efectoPowerUp;

    [Header("Item de munición que se otorga")]
    public ItemData itemData;

    private bool comprado = false; // <<<< NUEVO

    private void OnTriggerStay(Collider other)
    {
        if (comprado) return; // <<<< evita compras múltiples

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            PlayerDinero dineroJugador = other.GetComponent<PlayerDinero>();
            PlayerInventory inventario = other.GetComponent<PlayerInventory>();
            PlayerWeapons armas = other.GetComponent<PlayerWeapons>();

            if (dineroJugador != null && inventario != null && dineroJugador.dinero >= precio)
            {
                comprado = true; // <<<< asegura una sola compra

                dineroJugador.RestarDinero(precio);
                ActivarPowerUp(other.gameObject);

                if (itemData != null)
                {
                    if (!inventario.IsUnlocked(itemData.type))
                    {
                        inventario.UnlockWeapon(itemData.type);
                        armas?.Equip(itemData.type);
                    }

                    inventario.AddItem(itemData);
                }
                else
                {
                    Debug.LogWarning("No se asignó ningún ItemData al PowerUp.");
                }

                Debug.Log("PowerUp comprado por $" + precio);
                Destroy(gameObject); // Destruir el objeto tras comprar
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







