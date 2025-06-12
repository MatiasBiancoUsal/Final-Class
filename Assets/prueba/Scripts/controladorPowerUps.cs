using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controladorPowerUps : MonoBehaviour
{
    public GameObject gun;

    public GameObject gomaBullet;
    public bool gomaBulletIsStored = false;


    public static controladorPowerUps Instance;
    public bool descargaInsaciableIsStored = false;

    private void Awake()
    {

        
        if (controladorPowerUps.Instance == null)
        {
            controladorPowerUps.Instance = this;
            DontDestroyOnLoad(this.gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }

        if (controladorPowerUps.Instance.GetComponent<controladorPowerUps>().descargaInsaciableIsStored)
        {
            ActivateDescargaInsaciable();
            Debug.Log("funcooooooooooo");
        }
    }

    public void BalasDeGoma()
    {
        gun.GetComponent<gun>().bullet = gomaBullet;
    }
    public void agarroBalaDeGoma()
    {
        gomaBulletIsStored = true;  
    }

    #region Descarga Insaciable
    public void agarroDI()
    {
        descargaInsaciableIsStored = true;
    }

    public void ActivateDescargaInsaciable()
    {
        DescargaInsaciable();
        Invoke("StopDescargaInsaciable", 15.0f);
    }
    private void DescargaInsaciable()
    {
        gun.GetComponent<gun>().timeBetweenShooting = gun.GetComponent<gun>().timeBetweenShooting / 1.3f;
        Debug.Log(gun.GetComponent<gun>().timeBetweenShooting);
    }
    private void StopDescargaInsaciable()
    {
        gun.GetComponent<gun>().timeBetweenShooting = gun.GetComponent<gun>().timeBetweenShooting * 1.3f;
        Debug.Log(gun.GetComponent<gun>().timeBetweenShooting);
    }
    #endregion
}
