using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Vida simple: un golpe basta para romper el jarrón.
[RequireComponent(typeof(Collider))]
public class VaseHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 1;
    private int _hp;

    private Vase _vase;   // referencia al script Vase (loot)

    private void Awake()
    {
        _hp = maxHealth;
        _vase = GetComponent<Vase>();   // puede ser nulo, por si no suelta loot
    }

    /* ----- IDamageable ----- */
    public void TakeDamage(int amount)
    {
        _hp -= amount;
        if (_hp <= 0) Break();
    }
    /* ----------------------- */

    private void Break()
    {
        _vase?.DropLoot();   // 25 % de probabilidad
        Destroy(gameObject); // añade partículas o sfx si quieres
    }

    /* Seguridad: si prefieres rotura por colisión directa   */
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Projectile"))
            Break();
    }
}
