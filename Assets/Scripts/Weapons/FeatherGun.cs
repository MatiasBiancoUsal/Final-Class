using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherGun : WeaponBase
{
    [Header("Proyectil")]
    public GameObject projectilePrefab;
    public Transform muzzle;

    private PlayerTripleShot _tripleShot;

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.FeatherGun;

        // Cacheamos el componente para no buscar cada vez
        _tripleShot = _inventory.GetComponent<PlayerTripleShot>()
                     ?? _inventory.GetComponentInChildren<PlayerTripleShot>();
    }

    public override void TryShoot()
    {
        if (!projectilePrefab || !muzzle)
            return;

        // 1) Comprueba cooldown y munición
        if (!CanShoot)
            return;

        // 2) Ejecuta el gasto de bala, el cooldown y dispara la animación "Shoot"
        base.TryShoot();

        // 3) Genera los proyectiles
        var tripleShot = _inventory.GetComponent<PlayerTripleShot>();
        if (tripleShot != null && tripleShot.tripleShotActive)
        {
            FireProjectileAtAngle(0);
            FireProjectileAtAngle(-15);
            FireProjectileAtAngle(15);
        }
        else
        {
            FireProjectileAtAngle(0);
        }
    }


    private void FireProjectileAtAngle(float angleOffset)
    {
        // rotación yaw + offset
        Quaternion rot = Quaternion.Euler(
            0,
            muzzle.rotation.eulerAngles.y + angleOffset,
            0
        );
        // OFFSET ADELANTE (0.5m) y un poco arriba (si hiciera falta)
        Vector3 spawnPos = muzzle.position + muzzle.forward * 0.5f + Vector3.up * 0.1f;

        var proj = Instantiate(projectilePrefab, spawnPos, rot);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = proj.transform.forward * 24f;
            rb.useGravity = false;
        }

        // Ignorar colisión con TODO el jugador (root y hijos)
        var projCol = proj.GetComponent<Collider>();
        var playerRoot = _inventory.gameObject;
        foreach (var col in playerRoot.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(projCol, col);

        Debug.DrawRay(spawnPos, proj.transform.forward * 2f, Color.red, 1.5f);
        Debug.Log("Bala disparada en ángulo: " + angleOffset);
    }
}
