using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class LanceroHealth : MonoBehaviour, IDamageable
{
    [Header("Vida")]
    public int maxHealth = 200;
    private int _currentHealth;
    private bool _isDead;

    [Header("Defensa / Backstab")]
    [Range(0f, 1f)] public float defenseReduction = 0.5f;
    [Range(90f, 180f)] public float backstabAngleThreshold = 120f;

    [Header("Popup de Daño")]
    public DamagePopup damagePopupPrefab;

    [Header("Muerte")]
    public string deathTrigger = "Death";

    [Header("Referencias")]
    public Animator animator;  // El Animator del child
    public LanceroAI ai;        // El LanceroAI del parent

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

        Transform player = GameObject.FindWithTag("Player")?.transform;
        bool isBackstab = false;
        if (player != null)
        {
            Vector3 toPlayer = (player.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, toPlayer);
            isBackstab = angle > backstabAngleThreshold;
        }

        bool defenseActive = !(ai.IsAttacking || ai.IsInEndlag);
        int finalDamage = amount;
        if (defenseActive && !isBackstab)
            finalDamage = Mathf.RoundToInt(amount * defenseReduction);

        _currentHealth -= finalDamage;

        if (damagePopupPrefab != null)
            DamagePopup.Create(damagePopupPrefab, transform, finalDamage);

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        _isDead = true;

        if (ai != null) ai.enabled = false;
        var agent = GetComponentInParent<NavMeshAgent>();
        if (agent != null) agent.isStopped = true;
        foreach (var c in GetComponentsInChildren<Collider>())
            c.enabled = false;

        if (animator != null)
            animator.SetTrigger(deathTrigger);
        else
            Destroy(gameObject, 0.1f);
        //Destroy(transform.root.gameObject, 0.1f);

        //agregado por Maximo para el contador de enemigos, borrar o comentar ante cualquier conflicto
        roomManager.GetComponent<RoomManager>().numeroDeEnemigos--;
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject, 0.1f);
        //Destroy(transform.root.gameObject);
    }
}
