using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("¡Golpe registrado!");

        if (other.CompareTag("Rompible"))
        {
            Destroy(other.gameObject);
        }
    }
}

