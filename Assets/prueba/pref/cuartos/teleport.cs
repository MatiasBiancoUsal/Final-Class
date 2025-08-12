using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public float distance;

    public bool upDoor;
    public bool downDoor;
    public bool leftDoor;
    public bool rightDoor;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckTeleport()
    {
        if (upDoor)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + distance);
        }
        if (downDoor)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - distance);
        }
        if (leftDoor)
        {
            player.transform.position = new Vector3(player.transform.position.x - distance, player.transform.position.y, player.transform.position.z);
        }
        if (rightDoor)
        {
            player.transform.position = new Vector3(player.transform.position.x + distance, player.transform.position.y, player.transform.position.z);
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
