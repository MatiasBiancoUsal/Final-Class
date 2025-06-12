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

        if (inv.IsUnlocked(itemData.type))
        {
            // Ya tenía el arma → agregar munición
            inv.AddItem(itemData);

            print(itemData.type);
        }
        else
        {
            // No tenía el arma → desbloquear y opcionalmente dar munición
            inv.UnlockWeapon(itemData.type);

            print(itemData.type);

            // Si también da munición al desbloquear, se puede incluir esto:
            if (itemData.ammoGained > 0)
                inv.AddItem(itemData);
        }

        Destroy(gameObject);
    }
}


