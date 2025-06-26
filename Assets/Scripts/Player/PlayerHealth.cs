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
        // Si el escudo está activo, bloquea el daño
        if (_playerShield != null && _playerShield.IsShieldActive())
        {
            Debug.Log("¡Daño bloqueado por el ESCUDO desde PlayerHealth!");
            _playerShield.ConsumeShield(); // Desactiva y pone en cooldown
            return; // no se baja la vida
        }

        // Si no hay escudo, se aplica daño normalmente
        _currentHealth = Mathf.Max(_currentHealth - amount, 0);
        UpdateHealthUI();

        if (_currentHealth <= 0)
            Die();
    }

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
