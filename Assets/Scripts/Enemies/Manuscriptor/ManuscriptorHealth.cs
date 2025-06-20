using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class ManuscriptorHealth : MonoBehaviour, IDamageable
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
    public ManuscriptorAI ai;

    private bool _isDead;

    void Awake()
    {
        _currentHealth = maxHealth;
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

        // Para la IA y las colisiones
        if (agent != null) agent.isStopped = true;
        if (ai != null) ai.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Dispara el Trigger de muerte
        if (animator != null)
            animator.SetTrigger(deathTrigger);
        else
            Destroy(transform.root.gameObject, 0.1f);
    }

    // Llamado desde Animation Event en el último frame de Die
    public void OnDeathAnimationEnd()
    {
        Destroy(transform.root.gameObject);
    }
}
