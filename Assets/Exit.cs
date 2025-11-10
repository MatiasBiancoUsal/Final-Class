using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    int currentSceneIndex;
    public bool LastLevelBool = false;
    public GameObject roomManager;
    GameObject player;


    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        if (player.gameObject.tag == "Player" && roomManager.GetComponent<RoomManager>().cuartoCompletado)
        {
            //Anal... ytic
            EventSender.SendFinish(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().name);

            if (LastLevelBool == true)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(currentSceneIndex + 1);
            }
        }
    }
}
