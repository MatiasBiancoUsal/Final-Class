using UnityEngine;

/// <summary>Power-up: acelera la cadencia por un tiempo (Descarga Insaciable).</summary>
public class FireRateBoostPickup : MonoBehaviour
{
    [Tooltip("Duración en segundos")]
    public float duration = 15f;

    [Tooltip("Multiplicador de cooldown. 0.77 = ~30% más rápido.")]
    public float cooldownMultiplier = 0.77f;

    private void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<PlayerPowerUps>() ?? other.GetComponentInParent<PlayerPowerUps>();
        if (p == null) return;

        p.ActivateFireRate(duration, cooldownMultiplier);
        Destroy(gameObject);
    }
}
