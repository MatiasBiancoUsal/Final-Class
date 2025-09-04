using UnityEngine;

public class UnaVida : MonoBehaviour
{
    [Tooltip("Referencia al script PlayerHealth del jugador")]
    public PlayerHealth playerHealth;

    void Start()
    {
        if (playerHealth != null)
        {
            // Restaurar vida completa primero
            playerHealth.Heal(playerHealth.maxHealth);

            int vidaAUsar = playerHealth.maxHealth;

            // Si el desafío "OdioLosLunes" está activo, aplicar nerf del 90% - 
            if (DesafioManager.Instance.GetDesafio("OdioLosLunes"))
            {
                vidaAUsar = Mathf.Max(1, Mathf.RoundToInt(playerHealth.maxHealth * 0.1f)); // queda con el 10% de la vida
            }

            // Ajustar la vida actual usando TakeDamage
            playerHealth.TakeDamage(playerHealth.maxHealth - vidaAUsar);
        }
    }
}
