using UnityEngine;

public class TripleShotPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var tripleShot = other.GetComponent<PlayerTripleShot>();
            if (tripleShot != null)
                tripleShot.EnableTripleShot(10f); // dura 10 segundos

            Destroy(gameObject);
        }
    }
}
