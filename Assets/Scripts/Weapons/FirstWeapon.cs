using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Arma básica de ejemplo: golpe cuerpo a cuerpo.</summary>
public class FirstWeapon : WeaponBase
{
    [Header("Daño cuerpo a cuerpo")]
    public int damage = 10;
    public float range = 1.2f;
    public LayerMask enemyMask;

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.FirstWeapon;   // identifica su tipo
    }

    // Se llama desde TryShoot de WeaponBase a través del Animator
    public void DealDamage()                  // pon un Animation Event aquí
    {
        var hits = Physics.SphereCastAll(
            origin: transform.position,
            radius: 0.5f,
            direction: transform.forward,
            maxDistance: range,
            layerMask: enemyMask);

        foreach (var h in hits)
            h.transform.GetComponent<IDamageable>()?.TakeDamage(damage);
    }
}
