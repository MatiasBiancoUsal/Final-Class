using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [Header("GOs de las armas (hijos)")]
    public GameObject firstWeaponGO;   // cuchillo
    public GameObject featherGunGO;    // pluma

    private PlayerInventory _inventory;
    private AmmoHUD _hud;
    [SerializeField] private AmmoHUD ammoHUD;

    private PlayerInventory inv;

    private void Awake()
    {
        _inventory = GetComponent<PlayerInventory>();
        _hud = FindFirstObjectByType<AmmoHUD>();
    }

    private void Update()
    {
        /* ????????? Equipar armas ????????? */
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Equip(WeaponType.FirstWeapon);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Equip(WeaponType.FeatherGun);

        /* ????????? Disparar / Atacar ????????? */
        bool shooting = Input.GetKeyDown(KeyCode.G)      // tecla G
                     || Input.GetMouseButtonDown(0);     // clic izquierdo

        if (shooting)
        {
            WeaponBase wb = _inventory.equipped == WeaponType.FirstWeapon
                            ? firstWeaponGO.GetComponent<WeaponBase>()
                            : featherGunGO.GetComponent<WeaponBase>();

            wb?.TryShoot();
        }
    }

    /* Cambia el arma equipada y actualiza HUD */
    public void Equip(WeaponType w)
    {
        // Bloquea armas que aún no se han recogido
        if (!_inventory.IsUnlocked(w)) return;

        _inventory.equipped = w;

        firstWeaponGO.SetActive(w == WeaponType.FirstWeapon);
        featherGunGO.SetActive(w == WeaponType.FeatherGun);

        _hud?.OnWeaponEquipped(w);   // refresca balas en pantalla


        // Notificar al HUD
        if (ammoHUD != null)
        {
            ammoHUD.OnWeaponEquipped(w);
            Debug.Log("[PlayerWeapons] HUD actualizado para: " + w);
        }
        else
        {
            Debug.LogWarning("[PlayerWeapons] No se asignó AmmoHUD.");
        }


    }
}
