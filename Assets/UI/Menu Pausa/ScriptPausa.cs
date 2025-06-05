using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPausa : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;
    private bool Pausa = false;

    void Start()
    {
        ObjetoMenuPausa.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Pausa)
            {
                ActivarPausa();
            }
            else
            {
                Reanudar();
            }
        }
    }

    public void ActivarPausa()
    {
        ObjetoMenuPausa.SetActive(true);
        Time.timeScale = 0f;
        Pausa = true;

        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }

    public void Reanudar()
    {
        ObjetoMenuPausa.SetActive(false);
        Time.timeScale = 1f;
        Pausa = false;

        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

