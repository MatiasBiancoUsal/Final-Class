using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        if (player.gameObject.tag == "Player")
        {

            SceneManager.LoadScene(1);



        }
    }
}
