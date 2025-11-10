using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class ActualizarUsuario : MonoBehaviour
{

    bool ActivarInterfaz;
    string usuarioActual = "";
    string nuevoUsuario = "";



    Rect windowRect = new Rect(20, 20, 200, 200);


    // Start is called before the first frame update
    void Start()
    {
        ActivarInterfaz = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert) && Input.GetKey("u"))
        {
            ActivarInterfaz = !ActivarInterfaz;
        }
    }

    private void OnGUI()
    {

        if(ActivarInterfaz)
        {
            windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Cambio de usuarios");


        }

    }
    void DoMyWindow(int windowID)
    {
        GUILayout.Label("Usuario actual: " + AnalyticsService.Instance.GetAnalyticsUserID());
        nuevoUsuario = GUILayout.TextField(nuevoUsuario, 25);

        if (GUILayout.Button("Cambiar"))
            cambiarNuevoUsuario(nuevoUsuario);

        GUI.DragWindow();
    }



    void cambiarNuevoUsuario(string usuario)
    {
        usuarioActual = usuario;
        UnityServices.ExternalUserId = usuarioActual;
    }

}
