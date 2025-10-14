using UnityEngine;
using Unity.Services.Analytics;

public class BouncyAmmoPickup : MonoBehaviour
{
    [Tooltip("Duration in seconds")]
    public float duration = 15f;

    private void OnTriggerEnter(Collider other)
    {
        var p = other.GetComponent<PlayerPowerUps>() ?? other.GetComponentInParent<PlayerPowerUps>();
        if (p == null) return;

        p.ActivateBouncyAmmo(duration);
        Destroy(gameObject);
    }
}
