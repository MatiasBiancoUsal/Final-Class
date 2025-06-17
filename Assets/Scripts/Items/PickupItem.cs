using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    public WeaponType type = WeaponType.FeatherGun;
    public int ammoGained = 10; 

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (!inv || !itemData) return;

        if (!inv.IsUnlocked(itemData.type))
        {
            inv.UnlockWeapon(itemData.type);
            if (itemData.ammoGained > 0)
                inv.AddItem(itemData);

            // Forzar equipamiento si es un arma nueva
            var pw = other.GetComponent<PlayerWeapons>();
            if (pw) pw.SendMessage("Equip", itemData.type);
        }
        else
        {
            inv.AddItem(itemData);
        }

        Destroy(gameObject);
    }
}


