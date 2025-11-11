using UnityEngine;

public class BotonConfiguraciones : MonoBehaviour
{
    public GameObject panelControles; 
 
    public GameObject botonCerrarFondo;

    public void AbrirPanel()
    {
        panelControles.SetActive(true);
        botonCerrarFondo.SetActive(true); 
    }

    public void CerrarPanel()
    {
        panelControles.SetActive(false);
        botonCerrarFondo.SetActive(false);
    }
}