using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DamageFlashUI : MonoBehaviour
{
    [Header("Refs")]
    [Tooltip("Image que cubre toda la pantalla. Si no se asigna, usa la del propio GameObject.")]
    public Image overlay;

    [Header("Apariencia")]
    public Color flashColor = new Color(1f, 0f, 0f, 1f); // rojo
    [Range(0f, 1f)] public float maxAlpha = 0.35f;       // opacidad pico

    [Header("Tiempos (segundos)")]
    public float fadeIn = 0.06f;  // subida rápida
    public float hold = 0.05f;  // sostén muy breve
    public float fadeOut = 0.25f;  // bajada

    private Coroutine _co;

    private void Awake()
    {
        if (!overlay) overlay = GetComponent<Image>();
        if (overlay)
        {
            // que no bloquee clicks/UI y arranque invisible
            overlay.raycastTarget = false;
            var c = flashColor; c.a = 0f;
            overlay.color = c;
            overlay.enabled = true; // habilitado pero transparente
        }
    }

    /// <summary>
    /// Lanza el parpadeo. intensity = 1 usa maxAlpha, valores mayores/menores lo escalan.
    /// </summary>
    public void Flash(float intensity = 1f)
    {
        if (!overlay) return;
        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(CoFlash(Mathf.Max(0f, intensity)));
    }

    private IEnumerator CoFlash(float intensity)
    {
        float targetA = Mathf.Clamp01(maxAlpha * intensity);

        // Asegurar visible
        overlay.enabled = true;

        // Fade-in (unscaled)
        float t = 0f;
        Color baseCol = flashColor;
        baseCol.a = 0f;
        while (t < fadeIn)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeIn);
            var c = flashColor;
            c.a = Mathf.Lerp(0f, targetA, k);
            overlay.color = c;
            yield return null;
        }

        // Hold
        if (hold > 0f)
            yield return new WaitForSecondsRealtime(hold);

        // Fade-out
        t = 0f;
        while (t < fadeOut)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeOut);
            var c = flashColor;
            c.a = Mathf.Lerp(targetA, 0f, k);
            overlay.color = c;
            yield return null;
        }

        // Queda transparente (podés dejar enabled = true para evitar parpadeos de layout)
        var end = flashColor; end.a = 0f;
        overlay.color = end;
    }
}
