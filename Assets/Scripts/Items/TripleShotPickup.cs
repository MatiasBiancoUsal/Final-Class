using UnityEngine;

public class TripleShotPickup : MonoBehaviour
{
    [Tooltip("Duración del power-up en segundos")]
    public float duration = 10f;

    private void OnTriggerEnter(Collider other)
    {
        // 1) Intentamos obtener PlayerTripleShot en el collider o en sus padres
        var triple = other.GetComponent<PlayerTripleShot>()
                  ?? other.GetComponentInParent<PlayerTripleShot>();

        if (triple != null)
        {
            triple.EnableTripleShot(duration);
            Destroy(gameObject);
        }
    }
}

