using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Gestiona municiones y arma equipada del jugador.</summary>
public class PlayerInventory : MonoBehaviour
{
    /* ?? Evento: avisa al HUD cuando cambia la munici�n ?? */
    public event Action<WeaponType, int> OnAmmoChanged;

    /* ?? Munici�n por arma ?? */
    private readonly Dictionary<WeaponType, int> _ammo = new();

    /* ?? Armas desbloqueadas (por defecto solo el cuchillo) ?? */
    private readonly HashSet<WeaponType> _unlocked =
        new() { WeaponType.FirstWeapon };

    /* ?? Arma actualmente equipada ?? */
    public WeaponType equipped = WeaponType.FirstWeapon;

    /* ????????????? API p�blica ????????????? */

    /// <summary>Devuelve true si el arma est� desbloqueada.</summary>
    public bool IsUnlocked(WeaponType w) => _unlocked.Contains(w);

    /// <summary>Desbloquea un arma, si no lo estaba.</summary>
    public void UnlockWeapon(WeaponType w) => _unlocked.Add(w);

    /// <summary>A�ade munici�n (solo si ammoGained &gt; 0).</summary>
    public void AddItem(ItemData item)
    {
        if (!item || item.ammoGained <= 0) return;

        _ammo.TryAdd(item.type, 0);
        _ammo[item.type] += item.ammoGained;

        OnAmmoChanged?.Invoke(item.type, _ammo[item.type]);
    }

    /// <summary>Consume munici�n y devuelve true si hab�a suficiente.</summary>
    public bool TrySpendAmmo(WeaponType w, int amount = 1)
    {
        if (!_ammo.ContainsKey(w) || _ammo[w] < amount) return false;

        _ammo[w] -= amount;
        OnAmmoChanged?.Invoke(w, _ammo[w]);
        return true;
    }

    /// <summary>Devuelve la munici�n restante para un arma.</summary>
    public int GetAmmo(WeaponType w) =>
        _ammo.TryGetValue(w, out var a) ? a : 0;
}

