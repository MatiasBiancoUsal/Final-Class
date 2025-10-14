using UnityEngine;
using Unity.Services.Analytics;

public class PlayerShield : MonoBehaviour
{
    public bool isProtected = false;
    private bool onCooldown = false;
    public ParticleSystem shieldAura;

    public void ActivateShield()
    {
        if (!onCooldown)
        {
            isProtected = true;
            onCooldown = true;

            if (shieldAura != null)
                shieldAura.Play();
        }
    }

    public bool IsShieldActive()
    {
        return isProtected;
    }

    public void ConsumeShield()
    {
        if (isProtected)
        {
            isProtected = false;

            if (shieldAura != null)
                shieldAura.Stop();

            Invoke(nameof(ResetCooldown), 20f);
        }
    }

    private void ResetCooldown()
    {
        onCooldown = false;
    }
}
