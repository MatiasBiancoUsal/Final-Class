using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;   // asigna en Inspector

    private void Reset()
    {
        // Aseg�rate de que el collider sea Trigger para detectar la colisi�n
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponent<PlayerInventory>();
        if (!inv || !itemData) return;

        if (itemData.ammoGained > 0)
        {
            // Pickup de munici�n
            inv.AddItem(itemData);
        }
        else
        {
            // Pickup de arma: la desbloquea
            inv.UnlockWeapon(itemData.type);
        }

        Destroy(gameObject);   // desaparece la esfera
    }
}
