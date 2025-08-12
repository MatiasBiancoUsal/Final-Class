using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookOrderChecker : MonoBehaviour
{
    public DropSlot[] slots;
    public RewardManager rewardManager;

    public void CheckOrder()
    {
        foreach (DropSlot slot in slots)
        {
            if (slot.transform.childCount == 0)
            {
                Debug.Log($"Slot {slot.name} está vacío. Esperaba: {slot.correctBookName}");
                Debug.Log("¡Orden incorrecto!");
                return;
            }

            Transform child = slot.transform.GetChild(0);
            string childName = child.name.Replace("(Clone)", "").Trim();
            string expected = slot.correctBookName.Trim();

            if (!childName.Equals(expected, System.StringComparison.OrdinalIgnoreCase))
            {
                Debug.Log($"Slot {slot.name} contiene '{childName}' pero esperaba '{expected}'.");
                Debug.Log("¡Orden incorrecto!");
                return;
            }
        }

        Debug.Log("¡Orden correcto!");
        if (rewardManager != null) rewardManager.GiveReward(10);
    }
}

