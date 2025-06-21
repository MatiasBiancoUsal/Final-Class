using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    // Estado de protección y cooldown
    public bool isProtected = false;
    private bool onCooldown = false;

    // Referencia al sistema de partículas del aura visual
    public ParticleSystem shieldAura;

    /// <summary>
    /// Activa el escudo si no está en cooldown.
    /// Habilita la protección y activa el efecto visual.
    /// </summary>
    public void ActivateShield()
    {
        if (!onCooldown)
        {
            isProtected = true;
            onCooldown = true;
            Debug.Log("¡Escudo activado!");

            // Activar la aura visual si está asignada
            if (shieldAura != null)
            {
                Debug.Log("Activando aura...");
                shieldAura.Play();
            }
            else
            {
                Debug.LogWarning("shieldAura no está asignada en el Inspector.");
            }
        }
    }

    /// <summary>
    /// Simula recibir daño y verifica si el escudo está activo.
    /// Si está protegido, bloquea el daño y desactiva el escudo.
    /// </summary>
    /// <param name="amount">Cantidad de daño</param>
    public void ReceiveDamage(int amount)
    {
        if (isProtected)
        {
            Debug.Log("¡Daño bloqueado por escudo!");
            isProtected = false;

            // Detener la aura visual si está asignada
            if (shieldAura != null)
            {
                Debug.Log("Deteniendo aura...");
                shieldAura.Stop();
            }

            // Inicia el cooldown para volver a usar el escudo
            Invoke(nameof(ResetCooldown), 20f);
        }
        else
        {
            Debug.Log("Daño recibido: " + amount);
            // Acá podrías poner tu sistema de daño real
        }
    }

    /// <summary>
    /// Reinicia el cooldown, permitiendo volver a activar el escudo.
    /// </summary>
    private void ResetCooldown()
    {
        onCooldown = false;
        Debug.Log("Escudo listo para usarse de nuevo");
    }

    void Update()
    {
        // TEST: presioná K para simular daño
        if (Input.GetKeyDown(KeyCode.K))
        {
            ReceiveDamage(10);
        }
    }
}
