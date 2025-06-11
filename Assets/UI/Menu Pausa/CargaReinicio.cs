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

    void Start()
    {
        if (imagenCarga != null)
            imagenCarga.fillAmount = 0f;

        if (textoCarga != null)
            textoCarga.SetActive(false);

        if (confirmacionReinicio != null)
            confirmacionReinicio.SetActive(false); 
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            tiempoActual += Time.unscaledDeltaTime;
            float progreso = Mathf.Clamp01(tiempoActual / tiempoCarga);

            if (imagenCarga != null)
            {
                imagenCarga.fillAmount = progreso;
                imagenCarga.gameObject.SetActive(true);
            }

            if (textoCarga != null)
                textoCarga.SetActive(true);

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

            if (imagenCarga != null)
            {
                imagenCarga.fillAmount = 0f;
                imagenCarga.gameObject.SetActive(false);
            }

            if (textoCarga != null)
                textoCarga.SetActive(false);
        }
    }

    void MostrarConfirmacion()
    {
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (confirmacionReinicio != null)
            confirmacionReinicio.SetActive(true);
    }
}
