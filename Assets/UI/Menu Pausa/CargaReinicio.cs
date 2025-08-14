using UnityEngine;
using UnityEngine.UI;

public class CargaReinicio : MonoBehaviour
{
    public Image imagenCarga;
    public GameObject textoCarga;
    public float tiempoCarga = 2f;

    public GameObject confirmacionReinicio;

    private float tiempoActual = 0f;
    private bool reinicioEjecutado = false;
    private PauseManager _pause;

    void Start()
    {
        _pause = FindFirstObjectByType<PauseManager>();

        if (imagenCarga) imagenCarga.fillAmount = 0f;
        if (textoCarga)  textoCarga.SetActive(false);
        if (confirmacionReinicio) confirmacionReinicio.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            tiempoActual += Time.unscaledDeltaTime;
            float progreso = Mathf.Clamp01(tiempoActual / tiempoCarga);

            if (imagenCarga) { imagenCarga.fillAmount = progreso; imagenCarga.gameObject.SetActive(true); }
            if (textoCarga)  textoCarga.SetActive(true);

            if (progreso >= 1f && !reinicioEjecutado)
            {
                reinicioEjecutado = true;
                MostrarConfirmacion();
            }
        }
        else
        {
            tiempoActual = 0f;
            reinicioEjecutado = false;
            if (imagenCarga) { imagenCarga.fillAmount = 0f; imagenCarga.gameObject.SetActive(false); }
            if (textoCarga)  textoCarga.SetActive(false);
        }
    }

    void MostrarConfirmacion()
    {
        if (_pause && !PauseManager.IsPaused)
            _pause.TogglePause(); // pauses game + unlocks cursor + shows main panel

        if (confirmacionReinicio)
        {
            confirmacionReinicio.SetActive(true);
            // hide main panel if you want only the confirmation visible:
            if (_pause) _pause.GetComponent<PauseManager>()?.gameObject
                ?.GetComponentInChildren<Canvas>(true); // no-op, just to show place—main panel is already active via PauseManager
        }
    }
}
