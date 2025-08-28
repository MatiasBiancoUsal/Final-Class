using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [Header("Detección")]
    public float detectionRange = 12f;
    public string playerTag = "Player";

    [Header("Melee Attack")]
    public float meleeRange = 2f;
    public float meleeCooldown = 1.5f;
    public string meleeTrigger = "MeleeAttack";
    public int meleeDamage = 20;
    public Transform meleeHitPoint;
    public float meleeHitRadius = 1f;
    public LayerMask playerMask;
    [Tooltip("Multiplicador de velocidad de giro cuando está cerca y sin atacar")]
    public float meleeRotationMultiplier = 2f;

    [Header("Ranged Attack")]
    public float rangedMinRange = 6f;
    public float rangedMaxRange = 10f;
    public float rangedCooldown = 2f;
    public string rangedTrigger = "RangedAttack";
    public Transform projectileSpawnPoint;
    public GameObject projectilePrefab;
    public float projectileSpeed = 15f;

    [Header("Línea de visión")]
    public LayerMask obstacleMask;

    [Header("Rotación")]
    public float rotationSpeed = 5f;

    // Estados internos
    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;
    private float _nextMeleeTime;
    private float _nextRangedTime;
    private bool _isMeleeAttacking;
    private bool _isRangedAttacking;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        var go = GameObject.FindWithTag(playerTag);
        if (go != null) _player = go.transform;
    }

    void Update()
    {
        if (_player == null) return;

        // Si está ejecutando melee, no interrumpe hasta el evento OnMeleeAttackEnd
        if (_isMeleeAttacking) return;

        // Si está ejecutando ranged, sólo rota hasta OnRangedAttackEnd
        if (_isRangedAttacking)
        {
            RotateTowardsPlayer(rotationSpeed, true);
            return;
        }

        float dist = Vector3.Distance(transform.position, _player.position);
        bool inDetect = dist <= detectionRange;
        bool inMelee = dist <= meleeRange;
        bool inRanged = dist >= rangedMinRange && dist <= rangedMaxRange;
        bool hasLOS = !Physics.Linecast(
                             transform.position + Vector3.up * 1.2f,
                             _player.position + Vector3.up * 1.2f,
                             obstacleMask);
        bool canMelee = Time.time >= _nextMeleeTime;
        bool canRanged = Time.time >= _nextRangedTime;

        if (inDetect)
        {
            // 1) MELEE ZONE
            if (inMelee)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
                _animator.SetBool("IsMoving", false);

                if (canMelee)
                {
                    _isMeleeAttacking = true;
                    _animator.SetTrigger(meleeTrigger);
                    _nextMeleeTime = Time.time + meleeCooldown;
                }
                else
                {
                    // Rota rápido aguardando próximo melee
                    RotateTowardsPlayer(rotationSpeed * meleeRotationMultiplier, true);
                }
                return;
            }

            // 2) RANGED ZONE
            if (inRanged)
            {
                if (!hasLOS)
                {
                    Chase();
                }
                else
                {
                    _agent.isStopped = true;
                    _agent.ResetPath();
                    _animator.SetBool("IsMoving", false);

                    // Rota normalmente aguardando ranged
                    RotateTowardsPlayer(rotationSpeed, true);

                    if (canRanged)
                    {
                        _isRangedAttacking = true;
                        _animator.SetTrigger(rangedTrigger);
                        _nextRangedTime = Time.time + rangedCooldown;
                    }
                }
                return;
            }

            // 3) FUERA DE AMBOS RANGOS: persigue
            Chase();
        }
        else
        {
            // 4) FUERA DE DETECCIÓN: idle
            _agent.isStopped = true;
            _agent.ResetPath();
            _animator.SetBool("IsMoving", false);
        }
    }

    private void Chase()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
        _animator.SetBool("IsMoving", true);
    }

    /// <summary>
    /// Gira hacia el jugador. allowRotation habilita/deshabilita la rotación.
    /// </summary>
    private void RotateTowardsPlayer(float speed, bool allowRotation)
    {
        if (!allowRotation) return;
        Vector3 dir = _player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f) return;
        Quaternion tgt = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tgt, speed * Time.deltaTime);
    }

    // Animation Event: dispara el hit-sphere frontal
    public void DealMeleeDamage()
    {
        if (meleeHitPoint == null) return;
        Collider[] hits = Physics.OverlapSphere(
            meleeHitPoint.position,
            meleeHitRadius,
            playerMask);
        foreach (var h in hits)
            h.GetComponentInParent<IDamageable>()?.TakeDamage(meleeDamage);
    }

    // Animation Event: spawn del proyectil en ranged
    public void SpawnProjectile()
    {
        if (projectilePrefab == null || projectileSpawnPoint == null) return;
        var proj = Instantiate(
            projectilePrefab,
            projectileSpawnPoint.position,
            Quaternion.LookRotation(
                (_player.position - projectileSpawnPoint.position).normalized
            )
        );
        var rb = proj.GetComponent<Rigidbody>() ?? proj.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.linearVelocity = proj.transform.forward * projectileSpeed;
        if (proj.TryGetComponent<Collider>(out var c))
            foreach (var col in GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(c, col);
    }

    // Animation Event: fin de melee ⇒ desbloquea Update
    public void OnMeleeAttackEnd() => _isMeleeAttacking = false;
    // Animation Event: fin de ranged ⇒ desbloquea Update
    public void OnRangedAttackEnd() => _isRangedAttacking = false;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.cyan; Gizmos.DrawWireSphere(transform.position, meleeRange);
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, rangedMinRange);
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, rangedMaxRange);
        if (meleeHitPoint)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(meleeHitPoint.position, meleeHitRadius);
        }
    }
}
