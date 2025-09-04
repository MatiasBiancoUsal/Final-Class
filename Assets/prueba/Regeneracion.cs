using UnityEngine;

[AddComponentMenu("Combat/Regeneración (toggle con N)")]
public class Regeneracion : MonoBehaviour
{
    [Tooltip("Tecla para activar/desactivar la regeneración")]
    public KeyCode toggleKey = KeyCode.N;

    [Tooltip("Segundos sin recibir daño para empezar a curar")]
    public float outOfCombatDelay = 10f;

    [Tooltip("Cada cuántos segundos curar 1 HP mientras siga fuera de combate")]
    public float healInterval = 10f;

    [Tooltip("Cuánto curar por tick")]
    public int healAmount = 1;

    public bool isActive = false;

    float _nextTickTime = Mathf.Infinity;

    void Update()
    {
        // Toggle ON/OFF con N
        if (Input.GetKeyDown(toggleKey))
        {
            isActive = !isActive;
            if (isActive) _nextTickTime = Time.time + outOfCombatDelay;
        }

        if (!isActive) return;

        if (Time.time >= _nextTickTime)
        {
            // Llama Heal(int) si existe (no rompe si no está)
            SendMessage("Heal", healAmount, SendMessageOptions.DontRequireReceiver);
            _nextTickTime += healInterval; // sigue curando cada healInterval
        }
    }

    // Llamar cuando el player reciba daño
    public void NotifyDamaged()
    {
        if (!isActive) return;
        _nextTickTime = Time.time + outOfCombatDelay;
    }
}
