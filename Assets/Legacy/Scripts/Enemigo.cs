using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float vida;
    
    

   
    private void Update()
    {
        if(vida <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
