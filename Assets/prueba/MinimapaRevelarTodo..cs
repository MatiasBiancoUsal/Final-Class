using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Minimapa/Revelar Todo (ñ)")]
public class MinimapaRevelarTodo : MonoBehaviour
{
    [Header("Referencias")]
    public Camera minimapCamera;          // arrastrá MinimapCamera
    public GameObject minimapaChico;      // arrastrá HUD/…/MinimapaChico
    public GameObject minimapaGrande;     // arrastrá HUD/…/MinimapaGrande

    [Header("Teclas")]
    public KeyCode altKey = KeyCode.F9;   // por si ñ no entra

    [Header("Vista completa")]
    public float fullOrthoSize = 120f;    // tamaño para ver todo el mapa

    float _origSize = 20f;
    bool  _revealed = false;

    void Awake()
    {
        if (!minimapCamera)
            minimapCamera = GameObject.Find("MinimapCamera")?.GetComponent<Camera>();

        if (minimapCamera && !minimapCamera.orthographic)
            minimapCamera.orthographic = true;

        if (minimapCamera)
            _origSize = minimapCamera.orthographicSize;

        // estado inicial: chico encendido, grande apagado
        if (minimapaChico)  minimapaChico.SetActive(true);
        if (minimapaGrande) minimapaGrande.SetActive(false);
    }

    void Update()
    {
        // detectar "ñ" / "Ñ"
        var s = Input.inputString;
        bool ntilde = !string.IsNullOrEmpty(s) && (s.Contains("ñ") || s.Contains("Ñ"));

        if (ntilde || Input.GetKeyDown(altKey))
            ToggleReveal();
    }

    void ToggleReveal()
    {
        _revealed = !_revealed;

        // cambiar tamaño de cámara (todo / normal)
        if (minimapCamera)
            minimapCamera.orthographicSize = _revealed ? fullOrthoSize : _origSize;

        // alternar UI chico/grande
        if (minimapaChico)  minimapaChico.SetActive(!_revealed);
        if (minimapaGrande) minimapaGrande.SetActive(_revealed);

        // opcional: mostrar objetos “secretos”
        RevealSecretObjects(_revealed);
    }

    void RevealSecretObjects(bool show)
    {
        var trs = FindObjectsOfType<Transform>(true);
        foreach (var tr in trs)
        {
            var n = tr.name.ToLower();
            if (n.Contains("secret") || n.Contains("secreto") || tr.CompareTag("Secret") || tr.CompareTag("SecretRoom"))
            {
                var r = tr.GetComponent<Renderer>(); if (r) r.enabled = show;
                var img = tr.GetComponent<Image>();  if (img) img.enabled = show;
            }
        }
    }
}
