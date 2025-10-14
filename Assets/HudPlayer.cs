using UnityEngine;
using Unity.Services.Analytics;

[AddComponentMenu("UI/HUD Player")]
[DisallowMultipleComponent]
public class HudPlayer : MonoBehaviour
{
    [SerializeField] private PlayerWeapons playerWeapons;  
    [SerializeField] private AmmoHUD ammoHUD;              

    void Awake()
    {
        if (!playerWeapons) playerWeapons = GetComponent<PlayerWeapons>();
    }

    void Start()
    {
        
        if (!ammoHUD) ammoHUD = Object.FindFirstObjectByType<AmmoHUD>(FindObjectsInactive.Include);

        if (playerWeapons && ammoHUD)
            playerWeapons.ammoHUD = ammoHUD;
    }

    void OnEnable() => Start();
}
