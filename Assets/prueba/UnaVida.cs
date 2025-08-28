using UnityEngine;

public class UnaVida : MonoBehaviour
{
    [Tooltip("Referencia al script PlayerHealth del jugador")]
    public PlayerHealth playerHealth;

    [Tooltip("Vida inicial del jugador")]
    public int vidaInicial = 20;

    void Start()
    {
        if (playerHealth != null)
        {
            // Recibe da�o 
            playerHealth.TakeDamage(playerHealth.maxHealth - vidaInicial);
        }
    }
}

