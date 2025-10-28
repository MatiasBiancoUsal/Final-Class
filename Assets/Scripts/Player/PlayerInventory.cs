using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public event Action<WeaponType, int> OnAmmoChanged;

    private readonly Dictionary<WeaponType, int> _ammo = new();
    public WeaponType equipped = WeaponType.FirstWeapon;

    private readonly HashSet<WeaponType> _unlocked = new() { WeaponType.FirstWeapon };

    // --- NUEVO: mapeo de pools compartidas ---
    private static WeaponType AmmoKey(WeaponType w)
    {
        if (w == WeaponType.RapidFeatherGun) return WeaponType.FeatherGun; // comparten pool
        return w;
    }

    public bool IsUnlocked(WeaponType w) => _unlocked.Contains(w);
    public void UnlockWeapon(WeaponType w) => _unlocked.Add(w);

    // --- NUEVO: usa AddAmmo internamente ---
    public void AddItem(ItemData item)
    {
        if (!item) return;

        AddAmmo(item.type, item.ammoGained);

        if (item.type == WeaponType.FeatherGun && item.ammoGained > 0)
            TryUnlockPistolsByChance();
    }

    // --- NUEVO: entrada única para sumar munición respetando el mapeo ---
    public void AddAmmo(WeaponType ammoType, int amount)
    {
        if (amount <= 0) return;
        var key = AmmoKey(ammoType);
        _ammo.TryAdd(key, 0);
        _ammo[key] += amount;
        OnAmmoChanged?.Invoke(key, _ammo[key]);
    }

    private void TryUnlockPistolsByChance()
    {
        bool hasFeather = IsUnlocked(WeaponType.FeatherGun);
        bool hasRapid = IsUnlocked(WeaponType.RapidFeatherGun);
        if (hasFeather && hasRapid) return;

        float r = UnityEngine.Random.value;

        if (!hasFeather && !hasRapid)
        {
            if (r < 0.25f) UnlockWeapon(WeaponType.RapidFeatherGun);
            else UnlockWeapon(WeaponType.FeatherGun);
        }
        else if (!hasRapid)
        {
            if (r < 0.25f) UnlockWeapon(WeaponType.RapidFeatherGun);
        }
        else if (!hasFeather)
        {
            if (r >= 0.25f) UnlockWeapon(WeaponType.FeatherGun);
        }
    }

    public bool TrySpendAmmo(WeaponType w, int amount = 1)
    {
        var key = AmmoKey(w);
        if (!_ammo.ContainsKey(key) || _ammo[key] < amount) return false;
        _ammo[key] -= amount;
        OnAmmoChanged?.Invoke(key, _ammo[key]);
        return true;
    }

    public int GetAmmo(WeaponType w)
    {
        var key = AmmoKey(w);
        return _ammo.TryGetValue(key, out var a) ? a : 0;
    }
}
