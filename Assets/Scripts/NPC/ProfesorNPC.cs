using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfesorNPC : MonoBehaviour
{
    public string npcName = "Profesor";
    public GameObject minigameUI;

    void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            minigameUI.SetActive(true);
        }
    }
}
