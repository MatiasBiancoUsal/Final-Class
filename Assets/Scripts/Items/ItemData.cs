using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;

/// <summary>Describe un objeto recolectable (en este caso, munici�n).</summary>
[CreateAssetMenu(menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public WeaponType type;
    public int ammoGained = 15;
}
