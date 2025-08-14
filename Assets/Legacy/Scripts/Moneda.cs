using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    public int valor = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerDinero dinero = other.GetComponent<PlayerDinero>();
            if (dinero != null)
            {
                dinero.SumarDinero(valor);
                Debug.Log("Moneda recogida: " + valor + ". Total: " + dinero.dinero);
            }
            Destroy(gameObject);
        }
    }
}

