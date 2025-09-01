using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptPausa : MonoBehaviour
{
    [Header("Panels")]
    public GameObject ObjetoMenuPausa;        // main pause panel (assign the same one used by PauseManager)
    public GameObject ConfirmacionReinicio;   // confirmation modal

    [Header("Buttons")]
    public Button botonReiniciar;
    public Button botonSi;
    public Button botonNo;

    [Header("Optional")]
    public GameObject Jugador;

    private PauseManager _pause;

    void Start()
    {
        _pause = FindFirstObjectByType<PauseManager>();
        if (ObjetoMenuPausa) ObjetoMenuPausa.SetActive(false);
        if (ConfirmacionReinicio) ConfirmacionReinicio.SetActive(false);

        // Hook buttons
        if (botonReiniciar) botonReiniciar.onClick.AddListener(MostrarConfirmacion);
        if (botonSi)        botonSi.onClick.AddListener(Reiniciar);
        if (botonNo)        botonNo.onClick.AddListener(CerrarConfirmacion);
    }

    // Called by a UI "Resume" button (optional)
    public void Reanudar()
    {
        if (_pause && PauseManager.IsPaused)
            _pause.TogglePause(); // closes main panel & locks cursor
    }

    public void MostrarConfirmacion()
    {
        if (ObjetoMenuPausa)        ObjetoMenuPausa.SetActive(false);
        if (ConfirmacionReinicio)   ConfirmacionReinicio.SetActive(true);
        // We remain paused
    }

    public void CerrarConfirmacion()
    {
        if (ConfirmacionReinicio)   ConfirmacionReinicio.SetActive(false);
        if (ObjetoMenuPausa)        ObjetoMenuPausa.SetActive(true);
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f; // safety
        if (Jugador) Destroy(Jugador);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VolverInicio()
    {
        Time.timeScale = 1f; // safety
        if (Jugador) Destroy(Jugador);
        SceneManager.LoadScene("Prueba Menú");
    }
}
