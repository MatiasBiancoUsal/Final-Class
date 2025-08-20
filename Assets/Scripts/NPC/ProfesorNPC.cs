using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfesorNPC : MonoBehaviour
{
    public string npcName = "Profesor";
    public GameObject minigameUI;

    // agregado: abrir con E a distancia
    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 5f;
    public Transform player;

    BookOrderChecker checker;

    void Awake()
    {
        if (player == null && Camera.main != null) player = Camera.main.transform;
        if (minigameUI != null) checker = minigameUI.GetComponent<BookOrderChecker>();
        if (minigameUI != null) minigameUI.SetActive(false);
    }

    void Update()
    {
        if (minigameUI == null) return;
        if (Input.GetKeyDown(interactKey))
        {
            var refPos = player != null ? player.position : (Camera.main != null ? Camera.main.transform.position : transform.position);
            if (Vector3.Distance(transform.position, refPos) < interactDistance)
                minigameUI.SetActive(true);
        }
    }

    void OnMouseDown()
    {
        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            minigameUI.SetActive(true);
        }
    }
}
