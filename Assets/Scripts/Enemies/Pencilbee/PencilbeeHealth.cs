using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Services.Analytics;

[RequireComponent(typeof(Collider))]
public class PencilbeeHealth : MonoBehaviour, IDamageable
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
    public Animator animator;
    public NavMeshAgent agent;
    public PencilbeeAI ai;

    private bool _isDead;

    //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
    public GameObject roomManager;

    void Awake()
    {
        _currentHealth = maxHealth;

        // contador de enemigos
        if (roomManager != null)
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
        _isDead = true;

        //Analytic
        EventSender.SendEnemiesKilled(gameObject.name);

        if (agent != null) agent.isStopped = true;
        if (ai != null) ai.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        if (animator != null)
            animator.SetTrigger(deathTrigger);
        else
            Destroy(gameObject, 0.1f);

        if (roomManager != null)
            roomManager.GetComponent<RoomManager>().numeroDeEnemigos--;

    }

    // llamado desde el último frame de la animación de muerte
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject, 0.1f);
    }
}
