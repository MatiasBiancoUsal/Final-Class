using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vase : MonoBehaviour
{
    public GameObject monedaPrefab;
    public ItemData featherAmmoItem;

    [Range(0f, 1f)] public float chanceMoneda = 0.5f; 
    [Range(0f, 1f)] public float chanceItem = 0.25f; 

    public void DropLoot()
    {
        if (featherAmmoItem && Random.value < chanceItem)
        {
            var player = FindFirstObjectByType<PlayerInventory>();
            player?.AddItem(featherAmmoItem);
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
