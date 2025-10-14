using UnityEngine;
using Unity.Services.Analytics;

/// <summary>
/// Gestiona equipar armas y unifica la entrada de disparo (solo CLICK IZQUIERDO).
/// </summary>
public class PlayerWeapons : MonoBehaviour
{
    [Header("Armas (hijos)")]
    public GameObject firstWeaponGO;   // melee / cuchillo
    public GameObject featherGunGO;    // pistola de plumas

    private PlayerInventory _inventory;
    private AmmoHUD _hud;

    [SerializeField] public AmmoHUD ammoHUD; // opcional si ya tenés uno en escena

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
        _hud = FindFirstObjectByType<AmmoHUD>();
    }

    private void Start()
    {
        // Asegura visibilidad correcta al comenzar
        Equip(_inventory != null ? _inventory.equipped : WeaponType.FirstWeapon);
    }

    private void Update()
    {
        // Cambios de arma opcionales
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(WeaponType.FirstWeapon);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(WeaponType.FeatherGun);

        // DISPARO: solo click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            WeaponBase wb = _inventory.equipped == WeaponType.FirstWeapon
                ? firstWeaponGO.GetComponent<WeaponBase>()
                : featherGunGO.GetComponent<WeaponBase>();

            wb?.TryShoot();
        }
    }

    /// <summary>Cambia el arma equipada y actualiza HUD/visibilidad.</summary>
    public void Equip(WeaponType w)
    {
        if (_inventory == null) return;

        // No permitir equipar armas no desbloqueadas
        if (!_inventory.IsUnlocked(w)) w = WeaponType.FirstWeapon;

        _inventory.equipped = w;

        if (firstWeaponGO != null)  firstWeaponGO.SetActive(w == WeaponType.FirstWeapon);
        if (featherGunGO != null)   featherGunGO.SetActive(w == WeaponType.FeatherGun);

        _hud?.OnWeaponEquipped(w);
        ammoHUD?.OnWeaponEquipped(w);
    }
}
