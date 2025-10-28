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
    public GameObject rapidFeatherGunGO;   // NUEVO

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
        if (Input.GetKeyDown(KeyCode.Alpha3)) Equip(WeaponType.RapidFeatherGun);

        // DISPARO: solo click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            WeaponBase wb = null;

            if (_inventory.equipped == WeaponType.FirstWeapon && firstWeaponGO)
                wb = firstWeaponGO.GetComponent<WeaponBase>();
            else if (_inventory.equipped == WeaponType.FeatherGun && featherGunGO)
                wb = featherGunGO.GetComponent<WeaponBase>();
            else if (_inventory.equipped == WeaponType.RapidFeatherGun && rapidFeatherGunGO)
                wb = rapidFeatherGunGO.GetComponent<WeaponBase>();

            wb?.TryShoot();
        }
    }

    /// <summary>Cambia el arma equipada y actualiza HUD/visibilidad.</summary>
    public void Equip(WeaponType w)
    {
        if (!_inventory.IsUnlocked(w)) return;

        _inventory.equipped = w;

        if (firstWeaponGO) firstWeaponGO.SetActive(w == WeaponType.FirstWeapon);
        if (featherGunGO) featherGunGO.SetActive(w == WeaponType.FeatherGun);
        if (rapidFeatherGunGO) rapidFeatherGunGO.SetActive(w == WeaponType.RapidFeatherGun);

        _hud?.OnWeaponEquipped(w);
    }
}
