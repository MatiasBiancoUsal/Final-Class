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

    // NUEVO: referencia opcional al flash UI
    [Header("Feedback de daño")]
    public DamageFlashUI damageFlash;

    void Awake()
    {
        DontDestroyOnLoad(this);   


        _currentHealth = maxHealth;
        UpdateHealthUI();
        _playerShield = GetComponent<PlayerShield>();

        // Autolink por si te olvidás de arrastrarlo en el Inspector
        if (!damageFlash) damageFlash = FindFirstObjectByType<DamageFlashUI>();
    }

    public void TakeDamage(int amount)
    {
        // Escudo: bloquea daño
        if (_playerShield != null && _playerShield.IsShieldActive())
        {
            _playerShield.ConsumeShield();
            return;
        }

        int prev = _currentHealth;
        _currentHealth = Mathf.Max(_currentHealth - amount, 0);
        UpdateHealthUI();

        // NUEVO: parpadeo rojo (intensidad proporcional al daño relativo)
        if (_currentHealth < prev && damageFlash != null)
        {
            float intensity = Mathf.Clamp01((float)amount / Mathf.Max(1, maxHealth));
            damageFlash.Flash(intensity); // ←
        }

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

        
        Destroy(gameObject);


    }
}
