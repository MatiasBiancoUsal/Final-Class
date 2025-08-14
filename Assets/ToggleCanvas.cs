using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvas : MonoBehaviour
{
    public GameObject canvasDesafios; 
    private bool isActive = false;

    public void Toggle()
    {
        isActive = !isActive;
        canvasDesafios.SetActive(isActive);
    }
}