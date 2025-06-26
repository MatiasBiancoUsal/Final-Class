using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleShotPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var tripleShot = other.GetComponent<PlayerTripleShot>();
            if (tripleShot != null)
            {
                tripleShot.ActivatePowerup();
            }

            Destroy(gameObject); // El power-up desaparece
        }
    }
}
