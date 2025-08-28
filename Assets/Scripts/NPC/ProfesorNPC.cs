using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfesorNPC : MonoBehaviour
{
    public string npcName = "Profesor";
    public GameObject minigameUI;

    public KeyCode interactKey = KeyCode.E;
    public float interactDistance = 5f;
    public Transform player;

    BookOrderChecker checker;
    bool minigameCompleted = false; 
    bool isPlaying = false;         

    void Awake()
    {
        if (player == null && Camera.main != null)
            player = Camera.main.transform;

        if (minigameUI != null)
        {
            checker = minigameUI.GetComponentInChildren<BookOrderChecker>();
            minigameUI.SetActive(false);

            if (checker != null)
            {
                checker.onFinished -= OnMinigameFinished;
                checker.onFinished += OnMinigameFinished;
            }
            else
            {
                Debug.LogError("No encontré BookOrderChecker en minigameUI o sus hijos.");
            }
        }
    }

    void Update()
    {
        if (minigameUI == null || minigameCompleted) return;

        if (Input.GetKeyDown(interactKey))
        {
            var refPos = player != null ? player.position : (Camera.main != null ? Camera.main.transform.position : transform.position);

            if (Vector3.Distance(transform.position, refPos) < interactDistance)
            {
                OpenMinigame();
            }
        }
    }

    void OnMouseDown()
    {
        if (minigameUI == null || minigameCompleted) return;

        if (Vector3.Distance(transform.position, Camera.main.transform.position) < 5f)
        {
            OpenMinigame();
        }
    }

    void OpenMinigame()
    {
        if (minigameCompleted || isPlaying) return;

        isPlaying = true;
        minigameUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnMinigameFinished(bool win)
    {
        minigameCompleted = true; 
        isPlaying = false;        
        Debug.Log("Minijuego terminado, acceso bloqueado.");
    }
}
