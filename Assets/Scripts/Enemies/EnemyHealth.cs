using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Ajustes de vida")]
    [SerializeField] private int maxHealth = 100;
    private int _currentHealth;

    [Header("Popup de da�o")]
    [SerializeField] private DamagePopup damagePopupPrefab;

    private void Awake() => _currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;

        ShowPopup(amount);          // pasa el da�o REAL

        if (_currentHealth <= 0) Die();
    }

    private void ShowPopup(int dmg)
    {
        if (damagePopupPrefab)      // NUNCA uses Instantiate aqu�
            DamagePopup.Create(damagePopupPrefab, transform, dmg);
    }

    private void Die()
    {
        // efectos opcionales�
        Destroy(gameObject);
    }
}
