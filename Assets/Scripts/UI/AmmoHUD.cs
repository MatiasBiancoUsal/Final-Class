using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>HUD que muestra las balas del arma equipada.</summary>
public class AmmoHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private string format = "Balas: {0}";

    private PlayerInventory _inv;

    private void Start()
    {
        _inv = FindFirstObjectByType<PlayerInventory>();
        if (!_inv)
        {
            Debug.LogError("AmmoHUD: no se encontró PlayerInventory.");
            enabled = false; return;
        }

        // Suscripción a cambios de munición
        _inv.OnAmmoChanged += HandleAmmoChanged;

        // Mostrar munición inicial del arma equipada
        UpdateHUD(_inv.equipped, _inv.GetAmmo(_inv.equipped));
    }

    private void HandleAmmoChanged(WeaponType w, int amount)
    {
        if (w == _inv.equipped)
            UpdateHUD(w, amount);
    }

    /// <summary>Llamado desde PlayerWeapons al equipar arma.</summary>
    public void OnWeaponEquipped(WeaponType w)
    {
        UpdateHUD(w, _inv.GetAmmo(w));
    }

    private void UpdateHUD(WeaponType w, int amt)
    {
        // Si el arma equipada no usa balas (ej. cuchillo), oculta texto
        ammoText.enabled = (w != WeaponType.FirstWeapon);
        if (ammoText.enabled)
            ammoText.SetText(string.Format(format, amt));
    }

    private void OnDestroy()
    {
        if (_inv) _inv.OnAmmoChanged -= HandleAmmoChanged;
    }
}
