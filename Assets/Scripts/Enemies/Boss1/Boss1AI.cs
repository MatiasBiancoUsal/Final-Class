using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Boss1AI : MonoBehaviour
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

    // ---------------- Invocación ----------------
    [Header("Invocación de minions")]
    [Tooltip("Prefab del minion que aparecerá a la IZQUIERDA del boss")]
    public GameObject minionPrefabLeft;
    [Tooltip("Prefab del minion que aparecerá a la DERECHA del boss")]
    public GameObject minionPrefabRight;
    [Tooltip("Distancia lateral desde el boss para invocar")]
    public float summonSideDistance = 2.5f;
    [Tooltip("Desfase hacia adelante (opcional) al invocar")]
    public float summonForwardOffset = 0.0f;
    [Tooltip("Cooldown (s) entre invocaciones (spawnea 2: izq y der)")]
    public float summonCooldown = 12f;
    [Tooltip("Retraso inicial (s) antes de la primera invocación")]
    public float firstSummonDelay = 5f;
    [Tooltip("Radio de muestreo para clavar al NavMesh")]
    public float navMeshSampleRadius = 2f;
    [Tooltip("Áreas de NavMesh permitidas (0 = todas)")]
    public int navMeshAreaMask = NavMesh.AllAreas;

    private float _nextSummonTime;

    // --------------------------------------------

    NavMeshAgent _agent;
    Animator _animator;
    Transform _player;

    float _nextMeleeTime;
    bool _isMeleeAttacking;
    bool _inEndlag;
    float _endlagEndTime;

    // boludeces de max
    private bool enemyActive = false;
    Vector3 startpos; 

    void Awake()
    {
        //Max toco esto
        startpos = this.transform.position;

        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        var go = GameObject.FindWithTag(playerTag);
        if (go != null) _player = go.transform;

        _nextSummonTime = Time.time + firstSummonDelay;
    }

    void Update()
    {
        //Max toco esto
        if (startpos != this.transform.position)
        {
            enemyActive = true;
        }

        if (_player == null) return;

        // -------- INVOCACIÓN (no depende de animación, puede ocurrir aunque esté atacando) --------
        if (Time.time >= _nextSummonTime && enemyActive)
        {
            Debug.Log("summon");
            SummonMinions();
            _nextSummonTime = Time.time + summonCooldown;
        }

        // ------------------- LÓGICA DE MELEE Y MOVIMIENTO -------------------
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
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;
        Quaternion tgt = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tgt, rotationSpeed * Time.deltaTime);
    }

    // ---------------- Invocación ----------------

    private void SummonMinions()
    {
        // Posiciones ideales (local → mundo)
        Vector3 right = transform.right * summonSideDistance + transform.forward * summonForwardOffset;
        Vector3 left = -transform.right * summonSideDistance + transform.forward * summonForwardOffset;

        TrySpawnAt(minionPrefabLeft, transform.position + left);
        TrySpawnAt(minionPrefabRight, transform.position + right);
    }

    private void TrySpawnAt(GameObject prefab, Vector3 desiredWorldPos)
    {
        if (prefab == null) return;

        // Ajustar a NavMesh si es posible
        if (NavMesh.SamplePosition(desiredWorldPos, out var hit, navMeshSampleRadius, navMeshAreaMask))
            desiredWorldPos = hit.position;

        // Orientación inicial: misma del boss
        Quaternion rot = Quaternion.LookRotation(transform.forward, Vector3.up);
        Instantiate(prefab, desiredWorldPos, rot);
    }

    // --------------------------------------------------

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

        // Previsualización de puntos de invocación
        Gizmos.color = Color.yellow;
        Vector3 right = transform.position + transform.right * summonSideDistance + transform.forward * summonForwardOffset;
        Vector3 left = transform.position - transform.right * summonSideDistance + transform.forward * summonForwardOffset;
        Gizmos.DrawWireSphere(left, 0.3f);
        Gizmos.DrawWireSphere(right, 0.3f);
    }
}
