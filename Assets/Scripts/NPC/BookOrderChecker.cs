using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;

public class BookOrderChecker : MonoBehaviour
{
    public DropSlot[] slots;
    public RewardManager rewardManager;

    // agregado: cierre y notificación
    public GameObject minigameRoot;
    public Button verifyButton;
    public event Action<bool> onFinished;

    void Awake()
    {
        if (minigameRoot == null) minigameRoot = gameObject;
        if (verifyButton != null) verifyButton.onClick.AddListener(CheckOrder);
    }

    public void CheckOrder()
    {
        foreach (DropSlot slot in slots)
        {
            if (slot.transform.childCount == 0)
            {
                Debug.Log($"Slot {slot.name} está vacío. Esperaba: {slot.correctBookName}");
                Debug.Log("Orden incorrecto!");
                Close(false);
                return;
            }

            Transform child = slot.transform.GetChild(0);
            string childName = child.name.Replace("(Clone)", "").Trim();
            string expected = slot.correctBookName.Trim();

            if (!childName.Equals(expected, StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Slot {slot.name} contiene '{childName}' pero esperaba '{expected}'.");
                Debug.Log("Orden incorrecto!");
                Close(false);
                return;
            }
        }

        Debug.Log("Orden correcto!");
        if (rewardManager != null) rewardManager.GiveReward(10);
        Close(true);
    }

    public void Close(bool win)
    {
        if (minigameRoot != null) minigameRoot.SetActive(false);

        // Ocultar mouse y bloquear de nuevo
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        onFinished?.Invoke(win);
    }
}
