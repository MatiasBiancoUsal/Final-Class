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

        // Verificamos si el jugador tiene el power-up activo
        var tripleShot = _inventory.GetComponent<PlayerTripleShot>();
        if (tripleShot != null && tripleShot.tripleShotActive)
        {
            // Disparo triple con ángulos separados
            FireProjectileAtAngle(0);    // Centro
            FireProjectileAtAngle(-15);  // Izquierda
            FireProjectileAtAngle(15);   // Derecha
        }
        else
        {
            var proj = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

            // Le damos velocidad si tiene Rigidbody
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            if (rb != null)
                rb.velocity = proj.transform.forward * 20f;

            // Ignora colisiones con el player
            var projCol = proj.GetComponent<Collider>();
            foreach (var col in _inventory.GetComponentsInParent<Collider>())
                Physics.IgnoreCollision(projCol, col);
        }
    }

    private void FireProjectileAtAngle(float angleOffset)
    {
        Quaternion rotation = Quaternion.Euler(0, muzzle.rotation.eulerAngles.y + angleOffset, 0);
        var proj = Instantiate(projectilePrefab, muzzle.position, rotation);

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.velocity = proj.transform.forward * 20f;

        var projCol = proj.GetComponent<Collider>();
        foreach (var col in _inventory.GetComponentsInParent<Collider>())
            Physics.IgnoreCollision(projCol, col);

        Debug.DrawRay(muzzle.position, proj.transform.forward * 2f, Color.red, 2f);
    }
}
