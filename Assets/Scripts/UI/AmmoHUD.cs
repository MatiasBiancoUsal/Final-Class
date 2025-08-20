using TMPro;
using UnityEngine;

/// <summary>HUD que muestra las balas del arma equipada.</summary>
public class AmmoHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private string format = "Balas: {0}";

    private PlayerInventory _inv;

    private void Awake()
    {
        if (!ammoText)
            ammoText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void Start()
    {
        _inv = FindFirstObjectByType<PlayerInventory>();
        if (!_inv) { enabled = false; return; }

        _inv.OnAmmoChanged += HandleAmmoChanged;
        UpdateHUD(_inv.equipped, _inv.GetAmmo(_inv.equipped));
    }

    private void OnDestroy()
    {
        if (_inv) _inv.OnAmmoChanged -= HandleAmmoChanged;
    }

    private void HandleAmmoChanged(WeaponType w, int amount)
    {
        if (_inv && w == _inv.equipped)
            UpdateHUD(w, amount);
    }

    /// <summary>Llamado desde PlayerWeapons al equipar arma.</summary>
    public void OnWeaponEquipped(WeaponType w)
    {
        if (!_inv) return;
        UpdateHUD(w, _inv.GetAmmo(w));
    }

    private void UpdateHUD(WeaponType w, int amt)
    {
        if (!ammoText) return;
        bool show = (w != WeaponType.FirstWeapon);
        ammoText.enabled = show;
        if (show) ammoText.SetText(string.Format(format, amt));
    }
}
