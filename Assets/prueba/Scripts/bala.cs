using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject enemigo = collision.gameObject;

        if (collision.gameObject.tag == "Enemigo")
        {
            enemigo.GetComponent<Enemigo>().vida -= damage;
            Destroy(this.gameObject);
            Debug.Log("colisiona, " + "La vida del enemigo: " + enemigo.GetComponent<Enemigo>().vida);

        }
    }
}
