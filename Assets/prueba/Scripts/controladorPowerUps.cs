using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controladorPowerUps : MonoBehaviour
{
    public static controladorPowerUps Instance;
    public bool descargaInsaciableIsStored = false;

    private void Awake()
    {
        if (controladorPowerUps.Instance == null)
        {
            controladorPowerUps.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    public void agarroDI()
    {
        descargaInsaciableIsStored = true;
    }

   
}
