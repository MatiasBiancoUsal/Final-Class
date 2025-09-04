using UnityEngine;

public class DesafioManager : MonoBehaviour
{
    public static DesafioManager Instance;

    // Lista de todos los desafíos (buffs y nerfs)
    private string[] desafios = {
        "RecreoCancelado",   // Nerf
        "MaestroEstricto",   // Nerf
        "ClasesNocturnas",   // Nerf
        "TintaExplosiva",    // Nerf
        "OdioLosLunes",      // Nerf
        "AprobadoCon10",     // Buff
        "ApuntesCompletos",  // Buff
        "MeriendaExtra",     // Buff
        "ComodinEscolar"     // Buff
    };

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Inicializar en PlayerPrefs si no existen
        foreach (string desafio in desafios)
        {
            if (!PlayerPrefs.HasKey(desafio))
                PlayerPrefs.SetInt(desafio, 0); // 0 = desactivado
        }
    }

    // Activa o desactiva un desafío
    public void SetDesafio(string nombre, bool activo)
    {
        if (ExisteDesafio(nombre))
        {
            PlayerPrefs.SetInt(nombre, activo ? 1 : 0);
            Debug.Log(nombre + " guardado en estado " + activo);
        }
        else
        {
            Debug.LogWarning("El desafío " + nombre + " no existe en la lista.");
        }
    }

    // Consulta si está activado
    public bool GetDesafio(string nombre)
    {
        if (ExisteDesafio(nombre))
            return PlayerPrefs.GetInt(nombre, 0) == 1;

        Debug.LogWarning("El desafío " + nombre + " no existe en la lista.");
        return false;
    }

    // Verificar si el desafío está en la lista
    private bool ExisteDesafio(string nombre)
    {
        foreach (string d in desafios)
        {
            if (d == nombre) return true;
        }
        return false;
    }
}
