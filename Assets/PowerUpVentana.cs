using UnityEngine;

public class PowerUpVentana : MonoBehaviour
{
    public GameObject ventanaPowerUps;

    public void AlternarVentana()
    {
        if (ventanaPowerUps != null)
        {
            ventanaPowerUps.SetActive(!ventanaPowerUps.activeSelf);
        }
    }
}
