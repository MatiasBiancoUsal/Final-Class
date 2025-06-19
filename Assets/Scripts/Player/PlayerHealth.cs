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

    void Awake()
    {
        _currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
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
        // 1) Destruye este GameObject (jugador + su UI hijo)
        Destroy(gameObject);
        // 2) Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
