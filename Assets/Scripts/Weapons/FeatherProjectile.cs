using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class FeatherProjectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 24f;
    public float lifeTime = 3f;
    public LayerMask enemyMask;

    [Header("AoE on enemy hit")]
    public float aoeRadius = 2.5f;
    public int damage = 20;
    public float knockbackForce = 800f;

    [Header("Bounces")]
    [SerializeField] private int maxBounces = 1;

    private Rigidbody _rb;
    private Collider _selfCol;
    private bool _bouncy;
    private int _bounces;
    private Vector3 _lastVelocity;

    public void Initialize(bool bouncy) => _bouncy = bouncy;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _selfCol = GetComponent<Collider>();

        _rb.useGravity = false;
        _rb.linearDamping = 0f;
        _rb.angularDamping = 0f;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // keep orientation stable (prevents tiny torque from changing the next normal)
        _rb.constraints |= RigidbodyConstraints.FreezeRotation;
    }

    private void Start()
    {
        _rb.linearVelocity = transform.forward * speed;
        _lastVelocity = _rb.linearVelocity;
        Destroy(gameObject, lifeTime);
        // Debug.Log("[Projectile] FeatherProjectile active");
    }

    private void FixedUpdate()
    {
        if (_rb.linearVelocity.sqrMagnitude > 0.0001f)
            _lastVelocity = _rb.linearVelocity;

        // keep constant speed
        if (_rb.linearVelocity.sqrMagnitude > 0.01f)
            _rb.linearVelocity = _rb.linearVelocity.normalized * speed;
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Player")) return;

        bool hitEnemy = ((1 << col.gameObject.layer) & enemyMask.value) != 0
                        || col.collider.CompareTag("Enemigo");

        if (hitEnemy)
        {
            Explode();
            Destroy(gameObject);
            return;
        }

        // Non-enemy surface
        if (_bouncy && _bounces < maxBounces)
        {
            var contact = col.GetContact(0);
            Vector3 n   = contact.normal.normalized;

            // reflect the PRE-IMPACT direction, not the potentially modified current velocity
            Vector3 inDir    = _lastVelocity.sqrMagnitude > 0.0001f ? _lastVelocity.normalized : transform.forward;
            Vector3 reflected = Vector3.Reflect(inDir, n).normalized;

            // move out of the surface using the contact point (robust depenetration)
            transform.position = contact.point + n * 0.05f;

            _rb.linearVelocity = reflected * speed;
            _bounces++;

            // ignore this collider for a short window to prevent re-contact "sliding"
            StartCoroutine(IgnoreColliderFor(col.collider, 0.15f));
            return;
        }

        // No bouncy left: just die on any non-enemy hit
        Destroy(gameObject);
    }

    private IEnumerator IgnoreColliderFor(Collider other, float seconds)
    {
        if (_selfCol && other)
        {
            Physics.IgnoreCollision(_selfCol, other, true);
            yield return new WaitForSeconds(seconds);
            if (_selfCol && other) Physics.IgnoreCollision(_selfCol, other, false);
        }
    }

    private void Explode()
    {
        var hits = Physics.OverlapSphere(transform.position, aoeRadius, enemyMask);
        foreach (var h in hits)
        {
            h.GetComponent<IDamageable>()?.TakeDamage(damage);
            var rb = h.attachedRigidbody;
            if (rb) rb.AddExplosionForce(knockbackForce, transform.position, aoeRadius);
        }
    }
}
