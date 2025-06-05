using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class WeaponBase : MonoBehaviour
{
    [Header("Ajustes comunes")]
    public WeaponType weaponType = WeaponType.FirstWeapon;
    public float cooldown = 0.75f;

    [Tooltip("¿Esta arma gasta munición?")]
    public bool usesAmmo = true;

    protected float _nextShotTime;
    protected PlayerInventory _inventory;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _inventory = GetComponentInParent<PlayerInventory>();
        _animator = GetComponent<Animator>();
    }

    public virtual bool CanShoot =>
        Time.time >= _nextShotTime &&
        (!usesAmmo || _inventory == null || _inventory.GetAmmo(weaponType) > 0);

    public virtual void TryShoot()
    {
        if (!CanShoot) return;

        _nextShotTime = Time.time + cooldown;

        if (usesAmmo) _inventory?.TrySpendAmmo(weaponType);
        _animator?.SetTrigger("Shoot");
    }
}
