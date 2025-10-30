using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class BombibumHealth : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    public int maxHealth = 100;
    private int _currentHealth;

    [Header("Popup de daño")]
    public DamagePopup damagePopupPrefab;

    [Header("Muerte")]
    [Tooltip("Nombre exacto del Trigger 'Death' en el Animator")]
    public string deathTrigger = "Death";

    [Header("Referencias")]
    public Animator animator;  // arrastra aquí tu Animator
    public NavMeshAgent agent;
    public BombibumAI ai;

    private bool _isDead;

    // agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
    public GameObject roomManager;

    void Awake()
    {
        _currentHealth = maxHealth;

        // contador de enemigos (no tocar)
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos++;
    }

    public void TakeDamage(int amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        if (damagePopupPrefab != null)
            DamagePopup.Create(damagePopupPrefab, transform, amount);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (_isDead) return;
        _isDead = true;

        // Analytic
        EventSender.SendEnemiesKilled(1, gameObject.name);

        // Parar IA y colisiones
        if (agent != null) agent.isStopped = true;
        if (ai != null) ai.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Muerte con animación (solo si NO explotó)
        if (animator != null)
            animator.SetTrigger(deathTrigger);
        else
            Destroy(gameObject, 0.1f);

        // contador de enemigos (no tocar)
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos--;
    }

    /// <summary>
    /// Destruye sin animación (usado por la explosión).
    /// </summary>
    public void ExplodeKill()
    {
        if (_isDead) return;
        _isDead = true;

        // Analytic
        EventSender.SendEnemiesKilled(1, gameObject.name);

        if (agent != null) agent.isStopped = true;
        if (ai != null) ai.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // contador de enemigos (no tocar)
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos--;

        Destroy(gameObject, 0.05f);
    }

    // Llamado desde Animation Event al final de Die
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject, 0.1f);
    }
}
