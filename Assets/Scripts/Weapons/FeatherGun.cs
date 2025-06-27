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
        weaponType = WeaponType.FeatherGun;
    }

    public override void TryShoot()
    {
        if (!projectilePrefab || !muzzle) return;

        // Validar si tiene balas
        if (!_inventory.TrySpendAmmo(weaponType, 1)) return;

        // Verificamos si el jugador tiene el power-up activo
        var tripleShot = _inventory.GetComponent<PlayerTripleShot>();

        if (tripleShot != null && tripleShot.tripleShotActive)
        {
            // Disparo triple
            FireProjectileAtAngle(0);     // Centro
            FireProjectileAtAngle(-15);   // Izquierda
            FireProjectileAtAngle(15);    // Derecha
        }
        else
        {
            FireProjectileAtAngle(0); // Solo disparo central
        }
    }

    private void FireProjectileAtAngle(float angleOffset)
    {
        Quaternion rotation = Quaternion.Euler(0, muzzle.rotation.eulerAngles.y + angleOffset, 0);
        var proj = Instantiate(projectilePrefab, muzzle.position, rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = proj.transform.forward * 24f;
            rb.useGravity = false;
        }

        var projCol = proj.GetComponent<Collider>();
        foreach (var col in _inventory.GetComponentsInParent<Collider>())
            Physics.IgnoreCollision(projCol, col);

        Debug.DrawRay(muzzle.position, proj.transform.forward * 2f, Color.red, 1.5f);
Debug.Log("Bala disparada en ángulo: " + angleOffset);

    }
    
}
