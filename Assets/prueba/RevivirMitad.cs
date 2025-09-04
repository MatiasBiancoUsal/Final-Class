using UnityEngine;

[AddComponentMenu("Combat/Revivir (mitad de vida, tecla M)")]
public class RevivirMitad : MonoBehaviour
{
    public KeyCode key = KeyCode.M;

    [Tooltip("Porcentaje de vida al revivir (0–1). Ej: 0.5 = 50%")]
    [Range(0.1f, 1f)] public float revivePercent = 0.5f;

    [Tooltip("Presioná M para armar el revive. Se consume 1 vez al morir.")]
    public bool armed = false;
    public bool used  = false;

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (!used) armed = true; // si querés toggle: armed = !armed;
            Debug.Log(armed ? "Revivir armado" : "Revivir desarmado");
        }
    }

    // Lo usa PlayerHealth al morir
    public bool ConsumeIfArmed()
    {
        if (used || !armed) return false;
        used = true;
        armed = false;
        return true;
    }
}
