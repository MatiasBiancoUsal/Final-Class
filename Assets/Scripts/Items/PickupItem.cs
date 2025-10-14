using UnityEngine;
using Unity.Services.Analytics;

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
        // 🔍 Prueba máxima: Verificamos si entra al trigger
        Debug.Log("TOCÓ: " + other.name + " | TAG: " + other.tag);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("⛔ No tiene el tag Player");
            return;
        }

        Debug.Log("✅ Tiene el tag Player");

        var inv = other.GetComponent<PlayerInventory>();
        if (!inv || !itemData) return;

        // ESCUDO
        if (itemData.type == WeaponType.Shield)
        {
            Debug.Log("🛡️ Intentando activar escudo..."); // para ver si entra

            var shield = other.GetComponent<PlayerShield>();
            if (shield != null)
            {
                shield.ActivateShield();
            }
            else
            {
                Debug.Log("⚠️ PlayerShield NO encontrado en el jugador");
            }

            Destroy(gameObject);
            return; // Salimos para no seguir con armas
        }

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
