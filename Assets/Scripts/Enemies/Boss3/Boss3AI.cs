using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Boss3AI : MonoBehaviour
{
    [Header("Detección")]
    public string playerTag = "Player";
    public float detectionRange = 15f;
    public LayerMask obstacleMask;

    [Header("Ataque Cuerpo a Cuerpo")]
    public float meleeRange = 2f;
    public float meleeCooldown = 1.5f;
    public float meleeEndlag = 0.5f;
    public string meleeTrigger = "Attack";
    public Transform meleeHitPoint;
    public Vector3 meleeHitOffset = Vector3.zero;
    public float meleeHitRadius = 1f;
    public LayerMask playerMask;
    public int meleeDamage = 20;

    [Header("Rotación")]
    public float rotationSpeed = 5f;

    // ------------------ Orbes / patrones ------------------
    [Header("Orbes")]
    [Tooltip("Prefabs posibles del proyectil (elige uno aleatorio por disparo)")]
    public GameObject[] orbPrefabs;
    [Tooltip("Velocidad de los proyectiles")]
    public float orbSpeed = 10f;
    [Tooltip("Duración de vida de cada proyectil")]
    public float orbLifeTime = 5f;
    [Tooltip("Altura a la que salen los orbes (respecto a la posición del boss)")]
    public float orbSpawnYOffset = 1.2f;
    [Tooltip("Velocidad de giro del propio proyectil (°/s)")]
    public float orbSpinSpeed = 360f;
    [Tooltip("Daño que aplica cada orbe al tocar al jugador")]
    public int orbDamage = 15;
    [Tooltip("Capa(s) del jugador para el daño")]
    public LayerMask orbPlayerMask;

    [Header("Patrones")]
    [Tooltip("Balas en un anillo completo (patrón 3)")]
    public int ringBulletCount = 12;
    [Tooltip("Segundos entre sub-oleadas dentro de un mismo patrón (p.ej. Cross → 0.2s → Cross...)")]
    public float subWaveDelay = 0.2f;

    [Header("Ciclo de patrones")]
    [Tooltip("Cuántos patrones seguidos ejecuta antes del cooldown largo")]
    public int patternsPerCycle = 3;
    [Tooltip("Pausa entre patrones (corta)")]
    public float betweenPatternsDelay = 2f;
    [Tooltip("Cooldown largo tras completar patternsPerCycle patrones")]
    public float longCooldown = 8f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;

    private float _nextMeleeTime;
    private bool _isMeleeAttacking;
    private bool _inEndlag;
    private float _endlagEndTime;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        var go = GameObject.FindWithTag(playerTag);
        if (go != null) _player = go.transform;
        else Debug.LogError($"Boss3AI: no se encontró ningún GameObject con tag '{playerTag}'");
    }

    void Start()
    {
        // Arranca el bucle de patrones de orbes
        StartCoroutine(OrbPatternsLoop());
    }

    void Update()
    {
        if (_player == null) return;

        // --- Lógica de melee y movimiento base (igual que antes) ---
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

    // Animation Events (si los usas)
    public void OnDealMeleeDamage()
    {
        if (meleeHitPoint == null) return;

        Vector3 center = meleeHitPoint.position + meleeHitOffset;
        Collider[] hits = Physics.OverlapSphere(center, meleeHitRadius, playerMask);
        foreach (var h in hits)
            h.GetComponentInParent<IDamageable>()?.TakeDamage(meleeDamage);
    }

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

    // -------------------- ORB PATTERNS --------------------

    private IEnumerator OrbPatternsLoop()
    {
        while (true)
        {
            for (int i = 0; i < patternsPerCycle; i++)
            {
                int pattern = Random.Range(0, 3); // 0,1,2
                switch (pattern)
                {
                    case 0: yield return StartCoroutine(Pattern_Cross3()); break;
                    case 1: yield return StartCoroutine(Pattern_Cross_X_Cross()); break;
                    case 2: yield return StartCoroutine(Pattern_Ring()); break;
                }

                if (i < patternsPerCycle - 1)
                    yield return new WaitForSeconds(betweenPatternsDelay);
            }

            yield return new WaitForSeconds(longCooldown);
        }
    }

    // Patrón 0: CRUZ × 3 (4 direcciones, 3 sub-oleadas → 12 proyectiles)
    private IEnumerator Pattern_Cross3()
    {
        FireCross();                 // N, S, E, O
        yield return new WaitForSeconds(subWaveDelay);
        FireCross();
        yield return new WaitForSeconds(subWaveDelay);
        FireCross();
    }

    // Patrón 1: CRUZ → X → CRUZ (4 + 4 + 4 = 12 proyectiles)
    private IEnumerator Pattern_Cross_X_Cross()
    {
        FireCross();
        yield return new WaitForSeconds(subWaveDelay);
        FireX();
        yield return new WaitForSeconds(subWaveDelay);
        FireCross();
    }

    // Patrón 2: ANILLO (ringBulletCount proyectiles equiespaciados)
    private IEnumerator Pattern_Ring()
    {
        FireRing(ringBulletCount);
        yield break;
    }

    private void FireCross()
    {
        Vector3 upOffset = Vector3.up * orbSpawnYOffset;
        Vector3 pos = transform.position + upOffset;

        SpawnOrb(pos, transform.forward);
        SpawnOrb(pos, -transform.forward);
        SpawnOrb(pos, transform.right);
        SpawnOrb(pos, -transform.right);
    }

    private void FireX()
    {
        Vector3 upOffset = Vector3.up * orbSpawnYOffset;
        Vector3 pos = transform.position + upOffset;

        // 45°, 135°, 225°, 315°
        SpawnOrb(pos, Quaternion.Euler(0, 45, 0) * Vector3.forward);
        SpawnOrb(pos, Quaternion.Euler(0, 135, 0) * Vector3.forward);
        SpawnOrb(pos, Quaternion.Euler(0, 225, 0) * Vector3.forward);
        SpawnOrb(pos, Quaternion.Euler(0, 315, 0) * Vector3.forward);
    }

    private void FireRing(int count)
    {
        Vector3 upOffset = Vector3.up * orbSpawnYOffset;
        Vector3 pos = transform.position + upOffset;

        float step = 360f / Mathf.Max(1, count);
        for (int i = 0; i < count; i++)
        {
            Vector3 dir = Quaternion.Euler(0, i * step, 0) * Vector3.forward;
            SpawnOrb(pos, dir);
        }
    }

    private void SpawnOrb(Vector3 worldPos, Vector3 dir)
    {
        if (orbPrefabs == null || orbPrefabs.Length == 0) return;
        GameObject prefab = orbPrefabs[Random.Range(0, orbPrefabs.Length)];
        var go = Instantiate(prefab, worldPos, Quaternion.LookRotation(dir, Vector3.up));

        // Configura el proyectil si tiene BossOrbProjectile
        if (go.TryGetComponent<BossOrbProjectile>(out var orb))
        {
            // Ignora colisión con el boss
            var myCols = GetComponentsInChildren<Collider>();
            orb.Init(dir, orbSpeed, orbSpinSpeed, orbLifeTime, orbDamage, worldPos.y, orbPlayerMask, myCols);
        }
        else
        {
            // fallback: darle velocidad si tiene Rigidbody
            if (go.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.useGravity = false;
                rb.velocity = dir.normalized * orbSpeed;
            }
        }
    }

    // ------------------------------------------------------
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

        // Altura de spawn de orbes
        Gizmos.color = Color.yellow;
        Vector3 c = transform.position + Vector3.up * orbSpawnYOffset;
        Gizmos.DrawWireSphere(c, 0.2f);
    }
}
