using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPausa : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;   
    private bool Pausa = false; 

    void Start()
    {
        // Asegurarse de que el menú está desactivado al inicio
        ObjetoMenuPausa.SetActive(false);
        Time.timeScale = 1f;
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
        Time.timeScale = 0f;  // Congela el tiempo del juego
        Pausa = true;
    }

    public void Reanudar()
    {
        ObjetoMenuPausa.SetActive(false);
        Time.timeScale = 1f;  // Restaura el tiempo del juego
        Pausa = false;
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
