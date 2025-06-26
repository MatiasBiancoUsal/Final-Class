using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTripleShot : MonoBehaviour
{
    public bool tripleShotActive = false;
    public float duration = 10f;

    public void ActivatePowerup()
    {
        tripleShotActive = true;
        Debug.Log("Triple Shot ACTIVADO!");
        StartCoroutine(TripleShotTimer());
    }

    private IEnumerator TripleShotTimer()
    {
        yield return new WaitForSeconds(duration);
        tripleShotActive = false;
        Debug.Log("Triple Shot FINALIZADO.");
    }
}
