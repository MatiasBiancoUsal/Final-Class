using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomMinimapa : MonoBehaviour
{
    public GameObject minimapaChico;  // UI del minimapa pequeño
    public GameObject minimapaGrande; // UI del minimapa zoom

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ActivarZoom(true);
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ActivarZoom(false);
        }
    }

    void ActivarZoom(bool activar)
    {
        minimapaChico.SetActive(!activar);
        minimapaGrande.SetActive(activar);
    }

    public class ZoomMinimapaCamara : MonoBehaviour
    {
        public Camera minimapaCamara;
        public float zoomNormal = 100f;
        public float zoomCentrado = 30f;
        public Vector3 posicionNormal;
        public Vector3 posicionCentro;

        void Update()
        {
            if (Input.GetKey(KeyCode.Tab))
            {
                minimapaCamara.orthographicSize = zoomCentrado;
                minimapaCamara.transform.position = posicionCentro;
            }
            else
            {
                minimapaCamara.orthographicSize = zoomNormal;
                minimapaCamara.transform.position = posicionNormal;
            }
        }
    }
}
