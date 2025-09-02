using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 100;
    private int _currentHealth;
    public Image healthBarFill;

    private PlayerShield _playerShield;

    void Awake()
    {
        _currentHealth = maxHealth;
        UpdateHealthUI();
        _playerShield = GetComponent<PlayerShield>();
    }

    public void TakeDamage(int amount)
    {
        // Escudo: bloquea daño
        if (_playerShield != null && _playerShield.IsShieldActive())
        {
            Debug.Log("¡Daño bloqueado por el ESCUDO desde PlayerHealth!");
            _playerShield.ConsumeShield();
            return;
        }

        // Aplicar daño
        _currentHealth = Mathf.Max(_currentHealth - amount, 0);
        UpdateHealthUI();

        // >>> CAMBIO: avisar a la regeneración que hubo daño (reinicia el contador)
        GetComponent<Regeneracion>()?.NotifyDamaged();
        // <<< FIN CAMBIO

        if (_currentHealth <= 0)
            Die();
    }

    // >>> CAMBIO: método que usa la regeneración (SendMessage("Heal", ...))
    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }
    // <<< FIN CAMBIO

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)_currentHealth / maxHealth;
    }

    private void Die()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
