using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class PantallaInicio : MonoBehaviour
{
    public GameObject inicio;         
    public GameObject menuPrincipal; 
    public GameObject seleccionPersonaje; 

    public void IrAlMenuPrincipal()
    {
        inicio.SetActive(false);
        menuPrincipal.SetActive(true);
    }

    public void IrSeleccionPersonaje()
    {
        menuPrincipal.SetActive(false);
        seleccionPersonaje.SetActive(true);
    }
}

