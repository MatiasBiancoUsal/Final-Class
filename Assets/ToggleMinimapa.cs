using UnityEngine;

public class ToggleMinimapa : MonoBehaviour
{
    public GameObject MinimapaChico;
    public GameObject MinimapaGrande;

    private bool grandeActivo = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            grandeActivo = !grandeActivo;

            MinimapaChico.SetActive(!grandeActivo);
            MinimapaGrande.SetActive(grandeActivo);
        }
    }
}
