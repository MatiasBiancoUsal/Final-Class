using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Material material;

     float distance = 50f;

    public GameObject roomManager;
    

    public bool upDoor;
    public bool downDoor;
    public bool leftDoor;
    public bool rightDoor;


    
     GameObject player;

   

    private void TeleportTo(Vector3 newPos)
    {
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
            player.transform.position = newPos;
            cc.enabled = true;
        }
        else
        {
            player.transform.position = newPos;
        }
    }

    public void CheckTeleport()
    {
        if (roomManager.GetComponent<RoomManager>().cuartoCompletado)
        {
            
           
                            
                            if (upDoor)
                            {
                                TeleportTo(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + distance));
                               // player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + distance);
           
                            }
                            else if (downDoor)
                            {
                                TeleportTo(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - distance));
                               // player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - distance);
                            }
                            else if (leftDoor)
                            {
                                TeleportTo(new Vector3(player.transform.position.x - distance, player.transform.position.y, player.transform.position.z));
                              //  player.transform.position = new Vector3(player.transform.position.x - distance, player.transform.position.y, player.transform.position.z);
                            }
                            else if (rightDoor)
                            {
                                TeleportTo(new Vector3(player.transform.position.x + distance, player.transform.position.y, player.transform.position.z));
                               // player.transform.position = new Vector3(player.transform.position.x + distance, player.transform.position.y, player.transform.position.z);
                            }
        }
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        player = collision.gameObject;

        if (collision.gameObject.tag == "Player")
        {
            CheckTeleport();

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        player = other.gameObject;

        if (player.gameObject.tag == "Player")
        {
            
           
            
            CheckTeleport();

        }
    }
}
