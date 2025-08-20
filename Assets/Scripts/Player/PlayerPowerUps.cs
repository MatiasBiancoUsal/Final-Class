using System.Collections;
using UnityEngine;

/// <summary>
/// Centraliza y aplica modificadores temporales de power-ups al jugador.
/// - FireRateMultiplier: reduce el cooldown de cualquier WeaponBase.
/// - BouncyAmmoActive: hace que las balas reboten una vez.
/// </summary>
[DisallowMultipleComponent]
public class PlayerPowerUps : MonoBehaviour
{
    [Header("Estados actuales (solo lectura)")]
    [SerializeField] private float fireRateMultiplier = 1f; // 1 = normal
    [SerializeField] private bool bouncyAmmoActive = false;

    private Coroutine _fireRateRoutine;
    private Coroutine _bouncyRoutine;

    public float FireRateMultiplier => fireRateMultiplier;
    public bool BouncyAmmoActive    => bouncyAmmoActive;

    /// <summary>Activa un multiplicador de cadencia temporal. Ej.: 0.70f = 30% más rápido.</summary>
    public void ActivateFireRate(float duration, float multiplier)
    {
        multiplier = Mathf.Clamp(multiplier, 0.1f, 2f);
        if (_fireRateRoutine != null) StopCoroutine(_fireRateRoutine);
        
        Debug.Log($"[PowerUps] FireRate x{multiplier} durante {duration}s");
        
        _fireRateRoutine = StartCoroutine(FireRateRoutine(duration, multiplier));
    }

    /// <summary>Activa balas que rebotan una vez durante 'duration' segundos.</summary>
    public void ActivateBouncyAmmo(float duration)
    {
        if (_bouncyRoutine != null) StopCoroutine(_bouncyRoutine);
        
        Debug.Log($"[PowerUps] Bouncy ammo durante {duration}s");
        
        _bouncyRoutine = StartCoroutine(BouncyRoutine(duration));
    }

    private IEnumerator FireRateRoutine(float duration, float multiplier)
    {
        fireRateMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        fireRateMultiplier = 1f;
        _fireRateRoutine = null;
    }

    private IEnumerator BouncyRoutine(float duration)
    {
        bouncyAmmoActive = true;
        yield return new WaitForSeconds(duration);
        bouncyAmmoActive = false;
        _bouncyRoutine = null;
    }
}
