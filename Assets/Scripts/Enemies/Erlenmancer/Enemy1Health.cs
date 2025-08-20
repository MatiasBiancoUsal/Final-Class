using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class Enemy1Health : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    [Tooltip("Vida máxima del enemigo")]
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    [Header("Popup de daño")]
    [Tooltip("Prefab de DamagePopup (el que usa DamagePopup.Create)")]
    [SerializeField] private DamagePopup damagePopupPrefab;

    [Header("Animación de muerte")]
    [Tooltip("Nombre del booleano IsDead en el Animator")]
    [SerializeField] private string deadBool = "IsDead";

    private Animator _animator;
    private NavMeshAgent _agent;
    private bool _isDead;

    //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
    public GameObject roomManager;

    void Awake()
    {
        _currentHealth = maxHealth;
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos ++;
    }

    public void TakeDamage(int amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;

        // Muestra el popup en posición aleatoria, siguiendo al enemigo
        if (damagePopupPrefab != null)
            DamagePopup.Create(damagePopupPrefab, transform, amount);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _isDead = true;
        // Desactiva NavMesh y colisiones
        if (_agent) _agent.isStopped = true;
        var cols = GetComponentsInChildren<Collider>();
        foreach (var c in cols) c.enabled = false;

        // Lanza la animación de muerte
        if (_animator != null)
            _animator.SetBool(deadBool, true);
        else
            Destroy(gameObject, 0.1f);


        //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos--;
    }

    // Llamar desde un Animation Event al final del clip de muerte
    public void OnDeathEnd()
    {
        Destroy(gameObject);
    }
}
