using UnityEngine;
using Unity.Services.Analytics;

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
        _tripleShot = GetComponentInParent<PlayerTripleShot>();
    }

    public override void TryShoot()
    {
        if (!projectilePrefab || !muzzle) return;
        if (!CanShoot) return;

        base.TryShoot();

        bool doTriple = _tripleShot && _tripleShot.tripleShotActive;

        if (doTriple)
        {
            FireProjectileAtAngle(0f);
            FireProjectileAtAngle(-15f);
            FireProjectileAtAngle(15f);
        }
        else
        {
            FireProjectileAtAngle(0f);
        }
    }

    private void FireProjectileAtAngle(float yawOffsetDegrees)
    {
        // Mantiene el pitch del muzzle y gira solo en Y el offset lateral
        Quaternion rot = Quaternion.AngleAxis(yawOffsetDegrees, Vector3.up) * muzzle.rotation;

        Vector3 spawnPos = muzzle.position + muzzle.forward * 0.5f;
        var go = Instantiate(projectilePrefab, spawnPos, rot);

        // Ignorar colisiones contra TODO el jugador
        var projCol = go.GetComponent<Collider>();
        var playerRoot = _inventory ? _inventory.gameObject : GetComponentInParent<PlayerInventory>()?.gameObject;
        if (projCol && playerRoot)
        {
            foreach (var col in playerRoot.GetComponentsInChildren<Collider>(true))
                Physics.IgnoreCollision(projCol, col);
        }

        // Configurar proyectil
        var proj = go.GetComponent<FeatherProjectile>();
        if (proj != null)
        {
            bool bouncy = _powerUps && _powerUps.BouncyAmmoActive;
            proj.Initialize(bouncy);
        }
        else
        {
            var rb = go.GetComponent<Rigidbody>();
            if (rb) { rb.useGravity = false; rb.velocity = go.transform.forward * 24f; }
        }
    }
}
