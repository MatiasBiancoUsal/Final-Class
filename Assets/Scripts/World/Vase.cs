using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

public class Vase : MonoBehaviour
{
    public GameObject monedaPrefab;
    public ItemData featherAmmoItem;

    [Range(0f, 1f)] public float chanceMoneda = 0.5f; 
    [Range(0f, 1f)] public float chanceItem = 0.25f;

    public void DropLoot()
    {
        var player = FindFirstObjectByType<PlayerInventory>();

        if (featherAmmoItem && Random.value < chanceItem)
        {
            if (player)
            {
                Debug.Log("[Vase] Entregando item al jugador");

                player.AddItem(featherAmmoItem);

                if (!player.IsUnlocked(featherAmmoItem.type))
                {
                    player.UnlockWeapon(featherAmmoItem.type);
                    Debug.Log("[Vase] Arma desbloqueada");

                    var weapons = player.GetComponent<PlayerWeapons>();
                    if (weapons != null)
                    {
                        weapons.Equip(featherAmmoItem.type);  // Asegurate que Equip sea public
                        Debug.Log("[Vase] Arma equipada automáticamente");
                    }
                }
            }
        }

        if (monedaPrefab && Random.value < chanceMoneda)
        {
            Instantiate(monedaPrefab, transform.position, Quaternion.identity);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DropLoot(); 
            Destroy(gameObject);
        }
    }
}
