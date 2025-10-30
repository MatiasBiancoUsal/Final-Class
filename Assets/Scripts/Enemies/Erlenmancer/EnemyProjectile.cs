using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    [Header("Da�o")]
    [Tooltip("Da�o �nico que hace (se aplica UNA vez).")]
    public int damage = 20;

    [Tooltip("Radio de explosi�n al impactar")]

    public enum Team { Enemy, Player }   // NUEVO

    [Header("Daño AOE")]

    public float aoeRadius = 3f;
    [Tooltip("A quién daña cuando el proyectil es ENEMIGO")]
    public LayerMask playerMask;
    [Tooltip("A quién daña cuando el proyectil es ALIADO (tras desvío)")]
    public LayerMask enemyMask;

    [Header("Efectos y vida")]

    [Tooltip("Prefab de part�culas de explosi�n")]

    public GameObject explosionVFX;
    public float lifetime = 5f;

    [Header("Colisión")]
    [Tooltip("Tiempo de gracia tras volverse aliado, sin colisionar con el Player")]
    public float friendlyGrace = 0.5f;

    bool _exploded = false;
    public Team team = Team.Enemy;     // NUEVO
    float _noPlayerCollisionsUntil = 0f;

    Collider _col;
    Rigidbody _rb;

    void Awake()
    {
        TryGetComponent(out _col);
        TryGetComponent(out _rb);
    }

    void Start() => Destroy(gameObject, lifetime);

    void OnCollisionEnter(Collision col)
    {
        if (_exploded) return;

        // 1) Si es aliado, ignorar colisiones con el Player durante y después del grace
        if (team == Team.Player)
        {
            // Ignorá si el choque es con el Player o algo bajo su root
            if (col.collider.CompareTag("Player") || col.transform.root.CompareTag("Player"))
                return;

            // Si todavía estamos en la ventana de gracia, ignorar cualquier colisión con Player
            if (Time.time < _noPlayerCollisionsUntil) return;
        }

        _exploded = true;
        Explode();
    }

    public void Redirect(Vector3 newDirection, float speed)
    {
        if (_rb == null) return;

        // 2) Volverse ALIADO
        team = Team.Player;

        // Dirección + velocidad
        Vector3 dir = newDirection.normalized;
        _rb.velocity = dir * speed;     // si usás rb.velocity, reemplazá aquí
        transform.forward = dir;

        // 3) Opcionalmente cambiar Tag/Layer (crealos en el proyecto)
        gameObject.tag = "PlayerProjectile"; // si creás el Tag
        int lp = LayerMask.NameToLayer("PlayerProjectile");
        if (lp >= 0) gameObject.layer = lp;

        // 4) Ignorar colisiones con el Player (todas sus colliders)
        var playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null && _col != null)
        {
            foreach (var pc in playerGO.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(_col, pc, true);
        }

        // 5) Empujarlo un toque hacia adelante para salir de la cápsula del Player
        transform.position += dir * 0.25f;

        // 6) Ventana de gracia para asegurar que no registra colisión con el Player
        _noPlayerCollisionsUntil = Time.time + friendlyGrace;

        // (Opcional) Cambiar color/material para feedback visual de “aliado”
        // GetComponentInChildren<MeshRenderer>()?.material.SetColor("_EmissionColor", Color.cyan);
    }

    private void Explode()
    {
        if (_col) _col.enabled = false;
        if (_rb != null) _rb.velocity = Vector3.zero;


        // 2) Anular cualquier velocidad restante
        if (TryGetComponent<Rigidbody>(out var rb))
            rb.velocity = Vector3.zero;

        // 3) VFX
        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // 4) Da�o AOE
        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, playerMask);
        foreach (var h in hits)
        {
            h.GetComponentInParent<IDamageable>()?.TakeDamage(damage);


            //Analytics = para saber quien fue el ultimo enemigo que golpeo al jugador.
            PlayerData playerData = h.GetComponentInParent<PlayerData>();
            if (playerData != null)
            {
                playerData.enemyLastHit = "ErlenmancerProyectil";


            }
        }

        // 5) Destruye r�pido (pero deja tiempo al VFX)

        if (explosionVFX != null)
            Instantiate(explosionVFX, transform.position, Quaternion.identity);

        // 7) AOE: si es aliado daña ENEMIGOS; si es enemigo daña al PLAYER
        LayerMask mask = (team == Team.Player) ? enemyMask : playerMask;

        //Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, mask);
        //foreach (var h in hits)
        //    h.GetComponentInParent<IDamageable>()?.TakeDamage(damage);


        Destroy(gameObject, 0.1f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = (team == Team.Player) ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);
    }
}
