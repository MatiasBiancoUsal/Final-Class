using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class LanceroAI : MonoBehaviour
{
    [Header("Detección")]
    public string playerTag = "Player";
    public float detectionRange = 15f;
    public LayerMask obstacleMask;

    [Header("Movimiento")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 3f;

    [Header("Ataque cuerpo a cuerpo")]
    public float meleeRange = 2f;
    public float attackCooldown = 4f;
    public float attackEndlag = 1f;
    public string attackTrigger = "Attack";

    [Header("Hitbox melee")]
    public Transform meleeHitPoint;
    public Vector3 meleeHitOffset = Vector3.zero;
    public float meleeHitRadius = 1f;
    public LayerMask playerMask;
    public int meleeDamage = 20;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;

    private float _nextAttackTime;
    private bool _isAttacking;
    private bool _inEndlag;
    private float _endlagEndTime;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        var go = GameObject.FindGameObjectWithTag(playerTag);
        if (go != null) _player = go.transform;

        _agent.speed = moveSpeed;
    }

    void Update()
    {
        if (_player == null) return;

        if (_isAttacking) return;
        if (_inEndlag)
        {
            if (Time.time >= _endlagEndTime) _inEndlag = false;
            else return;
        }

        float dist = Vector3.Distance(transform.position, _player.position);
        bool canAttack = Time.time >= _nextAttackTime;
        bool hasLOS = !Physics.Linecast(
                              transform.position + Vector3.up * 1.2f,
                              _player.position + Vector3.up * 1.2f,
                              obstacleMask);

        if (dist <= meleeRange && canAttack && hasLOS)
        {
            StartAttack();
            return;
        }

        if (dist <= detectionRange)
        {
            Chase();
            return;
        }

        Idle();
    }

    private void StartAttack()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _animator.SetBool("IsMoving", false);
        _animator.SetTrigger(attackTrigger);

        _isAttacking = true;
        _nextAttackTime = Time.time + attackCooldown;
    }

    public void OnDealMeleeDamage()
    {
        if (meleeHitPoint == null) return;

        Vector3 center = meleeHitPoint.position + meleeHitOffset;
        Collider[] hits = Physics.OverlapSphere(center, meleeHitRadius, playerMask);
        foreach (var h in hits)
        {
             h.GetComponentInParent<IDamageable>()?.TakeDamage(meleeDamage);
            

            //Analytics = para saber quien fue el ultimo enemigo que golpeo al jugador.
            PlayerData playerData = h.GetComponentInParent<PlayerData>();
            if (playerData != null) 
            { 
                playerData.enemyLastHit = gameObject.name.Length >= 7 ? gameObject.name.Substring(0, 7) : gameObject.name;
            
            
            }
        }
           
            
    }

    public void OnAttackEnd()
    {
        _isAttacking = false;
        _inEndlag = true;
        _endlagEndTime = Time.time + attackEndlag;
    }

    private void Chase()
    {
        _agent.isStopped = false;
        _agent.SetDestination(_player.position);
        _animator.SetBool("IsMoving", true);
        FaceMovementDirection();
    }

    private void Idle()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
        _animator.SetBool("IsMoving", false);
    }

    private void FaceMovementDirection()
    {
        Vector3 dir = _agent.desiredVelocity;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.01f) return;
        Quaternion tgt = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            tgt,
            rotationSpeed * Time.deltaTime
        );
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
            Gizmos.DrawWireSphere(
                meleeHitPoint.position + meleeHitOffset,
                meleeHitRadius
            );
        }
    }

    // Expuestos para la defensa pasiva
    public bool IsAttacking => _isAttacking;
    public bool IsInEndlag => _inEndlag;
}
