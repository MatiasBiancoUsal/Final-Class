using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Boss1Health : MonoBehaviour, IDamageable
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
    public Boss1AI ai;

    private bool _isDead;


    //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
    public GameObject roomManager;

    void Awake()
    {
        _currentHealth = maxHealth;

        //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
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

        // Para la IA y las colisiones
        if (agent != null) agent.isStopped = true;
        if (ai != null) ai.enabled = false;

        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        // Dispara el Trigger de muerte
        if (animator != null)
            animator.SetTrigger(deathTrigger);
        else
            Destroy(gameObject, 0.1f);
        // Destroy(transform.root.gameObject, 0.1f);

        //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos--;
    }

    // Llamado desde Animation Event en el último frame de Die
    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject, 0.1f);
        // Destroy(transform.root.gameObject);
    }
}
