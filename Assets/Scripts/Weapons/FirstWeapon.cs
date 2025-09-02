using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Arma b�sica de ejemplo: golpe cuerpo a cuerpo.</summary>
public class FirstWeapon : WeaponBase
{
    [Header("Da�o cuerpo a cuerpo")]
    public int damage = 10;
    public float range = 1.2f;
    public LayerMask enemyMask;

    protected override void Awake()
    {
        base.Awake();
        weaponType = WeaponType.FirstWeapon;   // identifica su tipo
    }

    // Se llama desde TryShoot de WeaponBase a trav�s del Animator
    public void DealDamage()
    {
        var hits = Physics.SphereCastAll(
            origin: transform.position,
            radius: 0.5f,
            direction: transform.forward,
            maxDistance: range,
            layerMask: enemyMask // <-- incluir tambi�n el layer de los proyectiles
        );

        foreach (var h in hits)
        {
            // 1) da�o a enemigos
            var dmg = h.transform.GetComponent<IDamageable>();
            if (dmg != null)
            {
                dmg.TakeDamage(damage);
                continue;
            }

            // 2) desviar proyectiles enemigos
            var proj = h.transform.GetComponent<EnemyProjectile>();
            if (proj != null)
            {
                var rb = h.transform.GetComponent<Rigidbody>();
                float speed = (rb != null) ? rb.velocity.magnitude : 15f;

                Vector3 dir = Camera.main.transform.forward;  // misma direcci�n de la c�mara
                proj.Redirect(dir, speed);
            }
        }
    }

}
