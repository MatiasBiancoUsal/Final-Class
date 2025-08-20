using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{


    public GameObject roomManager;
    GameObject player;



    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        if (player.gameObject.tag == "Player" && roomManager.GetComponent<RoomManager>().cuartoCompletado)
        {

            SceneManager.LoadScene(1);



        }
    }
}
