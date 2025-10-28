using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BombibumAI : MonoBehaviour
{
    [Header("Detección")]
    public string playerTag = "Player";
    public float detectionRange = 15f;
    public LayerMask obstacleMask;

    [Header("Explosión")]
    [Tooltip("Distancia para comenzar la carga de explosión")]
    public float explodeRange = 2.5f;
    [Tooltip("Duración de la 'carga' (escala y color) antes de explotar")]
    public float chargeDuration = 1.5f;
    [Tooltip("Radio de daño del estallido")]
    public float explodeRadius = 3.0f;
    [Tooltip("Daño al jugador")]
    public int explodeDamage = 40;
    [Tooltip("Capa(s) del jugador para aplicar daño")]
    public LayerMask playerMask;

    [Header("Señales visuales de carga")]
    [Tooltip("Multiplicador de escala durante la carga")]
    public float chargeScaleMultiplier = 1.6f;
    [Tooltip("Color al que ‘blanquea’ durante la carga")]
    public Color chargeColor = Color.white;
    [Tooltip("Renderers a teñir (si se deja vacío, busca MeshRenderers en hijos)")]
    public Renderer[] renderersToTint;

    [Header("Rotación")]
    public float rotationSpeed = 5f;

    private NavMeshAgent _agent;
    private Transform _player;
    private BombibumHealth _health;

    private bool _isCharging;
    private Vector3 _baseScale;
    private System.Collections.Generic.List<Material[]> _originalMats;
    private System.Collections.Generic.List<Color[]> _originalColors;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<BombibumHealth>();

        var go = GameObject.FindWithTag(playerTag);
        if (go != null) _player = go.transform;
        else Debug.LogError($"BombibumAI: no se encontró ningún GameObject con tag '{playerTag}'");

        _baseScale = transform.localScale;

        if (renderersToTint == null || renderersToTint.Length == 0)
            renderersToTint = GetComponentsInChildren<Renderer>();

        _originalMats = new System.Collections.Generic.List<Material[]>();
        _originalColors = new System.Collections.Generic.List<Color[]>();
        foreach (var r in renderersToTint)
        {
            var mats = r.materials;
            _originalMats.Add(mats);
            var cols = new Color[mats.Length];
            for (int i = 0; i < mats.Length; i++)
                cols[i] = mats[i].HasProperty("_Color") ? mats[i].color : Color.white;
            _originalColors.Add(cols);
        }
    }

    void Update()
    {
        if (_player == null) return;
        if (_isCharging) return; // durante la carga queda quieto

        float dist = Vector3.Distance(transform.position, _player.position);
        bool hasLOS = !Physics.Linecast(
                        transform.position + Vector3.up * 1.2f,
                        _player.position + Vector3.up * 1.2f,
                        obstacleMask);

        // ¿Listo para cargar/explotar?
        if (dist <= explodeRange && hasLOS)
        {
            StartCoroutine(ChargeAndExplode());
            return;
        }

        // ¿Persigo o me quedo quieto?
        if (dist <= detectionRange)
            Chase();
        else
            Idle();
    }

    private void Chase()
    {
        if (_agent.isStopped) _agent.isStopped = false;
        _agent.SetDestination(_player.position);
        RotateTowardsPlayer();
    }

    private void Idle()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dir = _player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f) return;
        Quaternion tgt = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, tgt, rotationSpeed * Time.deltaTime);
    }

    private IEnumerator ChargeAndExplode()
    {
        _isCharging = true;
        _agent.isStopped = true;
        _agent.ResetPath();

        // Lerp de escala + color
        float t = 0f;
        while (t < chargeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / chargeDuration);

            // Escala
            transform.localScale = Vector3.Lerp(_baseScale, _baseScale * chargeScaleMultiplier, k);

            // Color → blanco
            for (int r = 0; r < renderersToTint.Length; r++)
            {
                var mats = renderersToTint[r].materials;
                for (int i = 0; i < mats.Length; i++)
                {
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = Color.Lerp(_originalColors[r][i], chargeColor, k);
                }
            }

            yield return null;
        }

        // Explosión AOE
        Vector3 center = transform.position + Vector3.up * 0.5f;
        Collider[] hits = Physics.OverlapSphere(center, explodeRadius, playerMask, QueryTriggerInteraction.Ignore);
        for (int i = 0; i < hits.Length; i++)
            hits[i].GetComponentInParent<IDamageable>()?.TakeDamage(explodeDamage);

        // Matar sin animación (no usa Death)
        _health?.ExplodeKill();
    }

    void OnDisable()
    {
        // Por si muere durante la carga: restaurar escala/colores
        transform.localScale = _baseScale;

        for (int r = 0; r < renderersToTint.Length; r++)
        {
            var mats = renderersToTint[r].materials;
            for (int i = 0; i < mats.Length && i < _originalColors[r].Length; i++)
                if (mats[i].HasProperty("_Color"))
                    mats[i].color = _originalColors[r][i];
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, explodeRange);

        Gizmos.color = new Color(1f, 0.3f, 0.2f, 0.6f);
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, explodeRadius);
    }
}
