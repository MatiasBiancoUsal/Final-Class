using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    

    public LayerMask enemyMask;
    public float aoeRadius = 2.5f;
    public int damage = 20;
    public float knockbackForce = 800f;

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
