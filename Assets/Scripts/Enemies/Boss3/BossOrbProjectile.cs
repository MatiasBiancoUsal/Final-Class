using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BossOrbProjectile : MonoBehaviour
{
    [Header("Debug")]
    public bool alignToVelocity = true;

    private Rigidbody _rb;
    private Collider _col;

    private float _spinSpeed;
    private float _yLock;
    private int _damage;
    private LayerMask _playerMask;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<Collider>();

        // Para triggers fiables con el player: puedes usar collider como trigger.
        // Si prefieres colisión física, quita la línea de isTrigger y maneja OnCollisionEnter.
        _col.isTrigger = true;

        _rb.useGravity = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    /// <summary>
    /// Inicializa el proyectil.
    /// </summary>
    public void Init(Vector3 dir, float speed, float spinSpeed, float lifeTime, int damage,
                     float yLock, LayerMask playerMask, Collider[] ignoreThese)
    {
        _spinSpeed = spinSpeed;
        _yLock = yLock;
        _damage = damage;
        _playerMask = playerMask;

        // Ignorar colisiones con el emisor (boss) y su jerarquía
        if (ignoreThese != null)
        {
            foreach (var c in ignoreThese)
                Physics.IgnoreCollision(_col, c, true);
        }

        // Velocidad horizontal
        dir.y = 0f;
        _rb.velocity = dir.normalized * speed;

        // Orientación
        if (alignToVelocity && _rb.velocity.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(_rb.velocity.normalized, Vector3.up);

        // Vida
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Mantener altura (sin subir/bajar)
        Vector3 p = _rb.position;
        if (!Mathf.Approximately(p.y, _yLock))
        {
            p.y = _yLock;
            _rb.position = p;
        }

        // Asegurar velocidad sin componente Y
        Vector3 v = _rb.velocity;
        if (!Mathf.Approximately(v.y, 0f))
        {
            v.y = 0f;
            _rb.velocity = v;
        }
    }

    void Update()
    {
        // Spin visual
        if (_spinSpeed != 0f)
            transform.Rotate(Vector3.up, _spinSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        // ¿Es el jugador?
        int layer = other.gameObject.layer;
        bool isPlayer = (_playerMask.value & (1 << layer)) != 0;

        if (isPlayer)
        {
            other.GetComponentInParent<IDamageable>()?.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

    // Si prefieres colisión en lugar de trigger:
    // void OnCollisionEnter(Collision col) { ... }
}
