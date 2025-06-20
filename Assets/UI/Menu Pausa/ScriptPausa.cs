using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPausa : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;
    public GameObject ConfirmacionReinicio;

    public Button botonReiniciar;
    public Button botonSi;
    public Button botonNo;

    public GameObject Jugador;

    private bool Pausa = false;

    void Start()
    {
        ObjetoMenuPausa.SetActive(false);
        ConfirmacionReinicio.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        botonReiniciar.onClick.AddListener(MostrarConfirmacion);
        botonSi.onClick.AddListener(Reiniciar);
        botonNo.onClick.AddListener(CerrarConfirmacion);
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
        Time.timeScale = 0f;
        ObjetoMenuPausa.SetActive(true);
        Pausa = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Reanudar()
    {
        Time.timeScale = 1f;
        ObjetoMenuPausa.SetActive(false);
        ConfirmacionReinicio.SetActive(false);
        Pausa = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MostrarConfirmacion()
    {
        ObjetoMenuPausa.SetActive(false);
        ConfirmacionReinicio.SetActive(true);
    }

    public void CerrarConfirmacion()
    {
        ConfirmacionReinicio.SetActive(false);
        ObjetoMenuPausa.SetActive(true);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f;
        Destroy(Jugador);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
