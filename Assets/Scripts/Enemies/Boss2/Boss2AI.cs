using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Boss2AI : MonoBehaviour
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

    // ------------------ Invocación de Puños ------------------
    [Header("Puños invocados")]
    [Tooltip("Prefabs posibles del puño (elige uno aleatorio por invocación)")]
    public GameObject[] fistPrefabs;
    [Tooltip("Cada cuántos segundos invocar (default 5s)")]
    public float fistsInterval = 5f;
    [Tooltip("Cantidad de puños por oleada")]
    public int fistsPerWave = 1;
    [Tooltip("Radio del área alrededor del boss donde caen los puños")]
    public float fistAreaRadius = 8f;
    [Tooltip("Capa de suelo para raycast")]
    public LayerMask groundMask;
    [Tooltip("Altura extra sobre el piso al instanciar (pre-ataque visual)")]
    public float fistSpawnYOffset = 0.15f;

    [Header("Comportamiento del puño")]
    [Tooltip("Demora antes de subir (tiempo de aviso)")]
    public float fistPreDelay = 0.75f;
    [Tooltip("Altura que sube el puño")]
    public float fistRiseHeight = 3f;
    [Tooltip("Tiempo que tarda en subir")]
    public float fistRiseDuration = 0.25f;
    [Tooltip("Tiempo que permanece tras el golpe antes de destruirse")]
    public float fistLingerAfter = 0.2f;
    [Tooltip("Daño al jugador cuando golpea")]
    public int fistDamage = 30;
    [Tooltip("Radio del golpe al subir")]
    public float fistHitRadius = 1.5f;
    [Tooltip("Máscara del jugador para el golpe del puño")]
    public LayerMask fistPlayerMask;

    private float _nextFistsTime;

    // ---------------------------------------------------------

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

        _nextFistsTime = Time.time + fistsInterval;
    }

    void Update()
    {
        if (_player == null) return;

        // ----------- Invocación de puños por intervalo -----------
        if (Time.time >= _nextFistsTime)
        {
            SummonFists();
            _nextFistsTime = Time.time + fistsInterval;
        }

        // ------------------- Lógica de melee/movimiento -------------------
        if (_isMeleeAttacking) return;

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

        if (dist <= meleeRange && canMelee && hasLOS)
        {
            StartMeleeAttack();
            return;
        }

        if (dist <= detectionRange)
        {
            Chase();
            return;
        }

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

    // ----------------- Invocación de puños -----------------

    private void SummonFists()
    {
        if (fistPrefabs == null || fistPrefabs.Length == 0 || fistsPerWave <= 0)
            return;

        for (int i = 0; i < fistsPerWave; i++)
        {
            // Punto aleatorio dentro de un círculo en XZ
            Vector2 rnd = Random.insideUnitCircle * fistAreaRadius;
            Vector3 desired = transform.position + new Vector3(rnd.x, 10f, rnd.y); // 10 unidades arriba para raycast

            // Raycast DOWN al suelo
            if (Physics.Raycast(desired, Vector3.down, out RaycastHit hit, 50f, groundMask, QueryTriggerInteraction.Ignore))
            {
                Vector3 spawnPos = hit.point + Vector3.up * fistSpawnYOffset;
                GameObject prefab = fistPrefabs[Random.Range(0, fistPrefabs.Length)];
                var obj = Instantiate(prefab, spawnPos, Quaternion.identity);

                // Si el prefab tiene FistTrap, configuramos sus parámetros
                if (obj.TryGetComponent<FistTrap>(out var trap))
                {
                    trap.preAttackDelay = fistPreDelay;
                    trap.riseHeight = fistRiseHeight;
                    trap.riseDuration = fistRiseDuration;
                    trap.lingerAfter = fistLingerAfter;
                    trap.damage = fistDamage;
                    trap.hitRadius = fistHitRadius;
                    trap.playerMask = fistPlayerMask;
                }
            }
        }
    }

    // --------------------- Gizmos ---------------------
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

        // Área de invocación de puños
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.6f);
        Gizmos.DrawWireSphere(transform.position, fistAreaRadius);
    }
}
