using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Regla : WeaponBase
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
        {
            // Si es enemigo: daño (aplica +50% si DanioAl50 está activo)
            var dmg = h.transform.GetComponent<IDamageable>();
            if (dmg != null)
            {
                var buff = Object.FindFirstObjectByType<DanioAl50>(FindObjectsInactive.Include);
                int finalDamage = damage;
                if (buff) finalDamage = Mathf.RoundToInt(finalDamage * buff.Current);

                dmg.TakeDamage(finalDamage);
                continue;
            }

            // Si es un proyectil enemigo: redirigirlo
            var proj = h.transform.GetComponent<EnemyProjectile>();
            if (proj != null)
            {
                var rb = h.transform.GetComponent<Rigidbody>();
                float speed = rb != null ? rb.velocity.magnitude : 15f;
                Vector3 dir = Camera.main.transform.forward;

                proj.Redirect(dir, speed);
            }
        }
    }
}
