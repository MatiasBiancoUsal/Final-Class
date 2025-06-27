using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FeatherProjectile : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 24f;
    public float lifeTime = 3f;

    [Header("Ataque en área")]
    public float aoeRadius = 2.5f;
    public int damage = 20;
    public float knockbackForce = 800f;
    public LayerMask enemyMask;

    private Rigidbody _rb;

    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void Start()
    {
        _rb.velocity = transform.forward * speed;
        Destroy(gameObject, lifeTime);
    }

   private void OnCollisionEnter(Collision col)
{
    // Ignorar colisión si choca con el jugador
    if (col.gameObject.CompareTag("Player"))
        return;

    Explode();
    Destroy(gameObject);
}


    private void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, aoeRadius, enemyMask);
        foreach (var h in hits)
        {
            h.GetComponent<IDamageable>()?.TakeDamage(damage);
            var rb = h.GetComponent<Rigidbody>();
            if (rb) rb.AddExplosionForce(knockbackForce, transform.position, aoeRadius);
        }
    }
}
