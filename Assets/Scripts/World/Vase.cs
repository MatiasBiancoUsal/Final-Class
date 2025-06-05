using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Vase : MonoBehaviour
{
    public ItemData featherAmmoItem;
    [Range(0f, 1f)] public float dropChance = 0.25f;

    /* Llama VaseHealth al romperse */
    public void DropLoot()
    {
        if (!featherAmmoItem) return;
        if (Random.value > dropChance) return;

        var player = FindFirstObjectByType<PlayerInventory>();
        player?.AddItem(featherAmmoItem);
    }
}