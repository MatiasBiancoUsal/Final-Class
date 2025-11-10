using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPlayerAlive : MonoBehaviour
{

    public GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if((SceneManager.GetActiveScene().name == "Prueba Menu") && player != null)
        {
            Destroy(player);
        }

        if((SceneManager.GetActiveScene().name != "Prueba Menu") && (player == null))
        {

            SceneManager.LoadScene(2);
        }
    }
}
