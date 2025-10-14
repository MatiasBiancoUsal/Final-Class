using UnityEngine;
using Unity.Services.Analytics;

/// <summary>Base de todas las armas. Controla cooldown, animación y munición.</summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Ajustes comunes")]
    public WeaponType weaponType = WeaponType.FirstWeapon;
    [Tooltip("Cooldown base entre ataques/disparos (seg).")]
    public float cooldown = 0.75f;

    [Tooltip("¿Esta arma gasta munición?")]
    public bool usesAmmo = true;

    protected float _nextShotTime;
    protected PlayerInventory _inventory;
    protected Animator _animator;
    protected PlayerPowerUps _powerUps;

    protected virtual void Awake()
    {
        _inventory = GetComponentInParent<PlayerInventory>();
        _powerUps  = GetComponentInParent<PlayerPowerUps>();
        _animator  = GetComponent<Animator>();
    }

    /// <summary>Cooldown efectivo luego de aplicar power-ups.</summary>
    protected virtual float EffectiveCooldown =>
        cooldown * (_powerUps != null ? _powerUps.FireRateMultiplier : 1f);

    public virtual bool CanShoot =>
        Time.time >= _nextShotTime &&
        (!usesAmmo || _inventory == null || _inventory.GetAmmo(weaponType) > 0);

    public virtual void TryShoot()
    {
        if (!CanShoot) return;

        _nextShotTime = Time.time + EffectiveCooldown;

        if (usesAmmo) _inventory?.TrySpendAmmo(weaponType);
        _animator?.SetTrigger("Shoot");
    }
}
