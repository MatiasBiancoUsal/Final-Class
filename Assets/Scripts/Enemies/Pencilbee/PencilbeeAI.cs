using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PencilbeeAI : MonoBehaviour
{
    [Header("Detección")]
    [Tooltip("Tag del jugador en la escena")]
    public string playerTag = "Player";
    [Tooltip("Radio dentro del cual detecta y persigue al jugador")]
    public float detectionRange = 15f;
    [Tooltip("Capa que bloquea la visión (p.ej. Obstaculo)")]
    public LayerMask obstacleMask;

    [Header("Ataque Cuerpo a Cuerpo")]
    [Tooltip("Distancia máxima para iniciar el melee")]
    public float meleeRange = 2f;
    [Tooltip("Cooldown (s) entre ataques")]
    public float meleeCooldown = 1.5f;
    [Tooltip("Tiempo (s) de endlag después del ataque")]
    public float meleeEndlag = 0.5f;
    [Tooltip("Trigger en el Animator para el ataque")]
    public string meleeTrigger = "Attack";
    [Tooltip("Transform base para el hitbox melee")]
    public Transform meleeHitPoint;
    [Tooltip("Offset (local) del centro del hitbox respecto al punto anterior")]
    public Vector3 meleeHitOffset = Vector3.zero;
    [Tooltip("Radio del hitbox melee")]
    public float meleeHitRadius = 1f;
    [Tooltip("Capa(s) que representan al jugador para el hit")]
    public LayerMask playerMask;
    [Tooltip("Daño infligido por el golpe melee")]
    public int meleeDamage = 20;

    [Header("Rotación")]
    [Tooltip("Velocidad de giro al seguir al jugador")]
    public float rotationSpeed = 5f;

    NavMeshAgent _agent;
    Animator _animator;
    Transform _player;

    float _nextMeleeTime;
    bool _isMeleeAttacking;
    bool _inEndlag;
    float _endlagEndTime;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        var go = GameObject.FindWithTag(playerTag);
        if (go != null) _player = go.transform;
        else Debug.LogError($"ManuscriptorAI: no se encontró ningún GameObject con tag '{playerTag}'");
    }

    void Update()
    {
        if (_player == null) return;

        // 1) Si ejecutando melee, nada interrumpe hasta OnMeleeAttackEnd
        if (_isMeleeAttacking) return;

        // 2) Si en endlag, bloquea todo motion/rotación hasta que acabe
        if (_inEndlag)
        {
            if (Time.time >= _endlagEndTime) _inEndlag = false;
            else return;
        }

        float dist = Vector3.Distance(transform.position, _player.position);
        bool canMelee = Time.time >= _nextMeleeTime;
        bool hasLOS = !Physics.Linecast(
                            transform.position + Vector3.up * 1.2f,
                            _player.position + Vector3.up * 1.2f,
                            obstacleMask);

        // 3) Si estás dentro de rango melee y pasó cooldown y tienes visión:
        if (dist <= meleeRange && canMelee && hasLOS)
        {
            StartMeleeAttack();
            return;
        }

        // 4) Si estás dentro de detectionRange, persigue
        if (dist <= detectionRange)
        {
            Chase();
            return;
        }

        // 5) Fuera de detectionRange → idle
        Idle();
    }

    private void StartMeleeAttack()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger(meleeTrigger);
        _isMeleeAttacking = true;
        _nextMeleeTime = Time.time + meleeCooldown;
    }

    // Llamado desde Animation Event en el frame de daño
    public void OnDealMeleeDamage()
    {
        if (meleeHitPoint == null) return;

        Vector3 center = meleeHitPoint.position + meleeHitOffset;
        Collider[] hits = Physics.OverlapSphere(center, meleeHitRadius, playerMask);
        foreach (var h in hits)
            h.GetComponentInParent<IDamageable>()?.TakeDamage(meleeDamage);
    }

    // Llamado desde Animation Event en el último frame del clip Attack
    public void OnMeleeAttackEnd()
    {
        _isMeleeAttacking = false;
        _inEndlag = true;
        _endlagEndTime = Time.time + meleeEndlag;
    }

    private void Chase()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
        _animator.SetBool("IsMoving", true);
        RotateTowardsPlayer();
    }

    private void Idle()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _animator.SetBool("IsMoving", false);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dir = _player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f) return;
        Quaternion tgt = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tgt, rotationSpeed * Time.deltaTime);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
        if (meleeHitPoint)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(meleeHitPoint.position + meleeHitOffset, meleeHitRadius);
        }
    }
}
