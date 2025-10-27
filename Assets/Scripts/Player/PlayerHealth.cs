using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Services.Analytics;

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

        
        _currentHealth = Mathf.Max(_currentHealth - amount, 0);
        UpdateHealthUI();

       
        GetComponent<Regeneracion>()?.NotifyDamaged();
        

        if (_currentHealth <= 0)
            Die();
    }

    
    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, maxHealth);
        UpdateHealthUI();
    }
    

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = (float)_currentHealth / maxHealth;
    }

    private void Die()
    {
    // Revivir con porcentaje configurable si está armado (M) y no usado
    var revive = GetComponent<RevivirMitad>();
    if (revive && revive.ConsumeIfArmed())
    {
        int hp = Mathf.Max(1, Mathf.RoundToInt(maxHealth * Mathf.Clamp01(revive.revivePercent)));
        _currentHealth = hp;
        UpdateHealthUI();
        Debug.Log($"¡Reviviste con {hp}/{maxHealth} HP!");
        return; 
    }

        //Analytics
        int sceneID = SceneManager.GetActiveScene().buildIndex; 
        EventSender.SendDeathPlayer(sceneID);

        EventSender.SendEnemyThatKilledPlayer(GetComponent<PlayerData>().enemyLastHit);

        // re chequear esto psd Maximo
            
        SceneManager.LoadScene(2);
        Destroy(gameObject);
       

    }
    
}