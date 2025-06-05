using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Implementa la pluma que dispara tinta explosiva.</summary>
public class FeatherGun : WeaponBase
{
    [Header("Proyectil")]
    public GameObject projectilePrefab;
    public Transform muzzle;

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.FeatherGun;   // fuerza su tipo
    }

    /// <summary>Se llama desde un Animation Event en el clip de disparo.</summary>
    public void SpawnProjectile()
    {
        if (!projectilePrefab || !muzzle) return;

        var proj = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        // Ignora colisiones con el player
        var projCol = proj.GetComponent<Collider>();
        foreach (var col in _inventory.GetComponentsInParent<Collider>())
            Physics.IgnoreCollision(projCol, col);
    }
}
