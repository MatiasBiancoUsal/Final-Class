using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Daño")]
    [Tooltip("Daño único que hace (se aplica UNA vez).")]
    public int damage = 20;

    [Header("Daño AOE")]
    [Tooltip("Radio de explosión al impactar")]
    public float aoeRadius = 3f;
    [Tooltip("Capa(s) del jugador para la AOE")]
    public LayerMask playerMask;

    [Header("Efectos y vida")]
    [Tooltip("Prefab de partículas de explosión")]
    public GameObject explosionVFX;
    [Tooltip("Autodestruye tras X segundos si no choca")]
    public float lifetime = 5f;

    bool _exploded = false;

    void Start() => Destroy(gameObject, lifetime);

    void OnCollisionEnter(Collision col)
    {
        if (_exploded) return;
        _exploded = true;

        Explode();
    }

    private void Explode()
    {
        // 1) Desactivar collider para que no vuelva a colisionar
        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;

        // 2) Anular cualquier velocidad restante
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = Vector3.zero;

        // 3) VFX
        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // 4) Daño AOE
        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, playerMask);
        foreach (var h in hits)
            h.GetComponentInParent<PlayerHealth>()?.TakeDamage(damage);

        // 5) Destruye rápido (pero deja tiempo al VFX)
        Destroy(gameObject, 0.1f);
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
