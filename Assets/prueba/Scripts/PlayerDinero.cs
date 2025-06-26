using UnityEngine;
using TMPro;

public class PlayerDinero : MonoBehaviour
{
    public int dinero = 0;
    public TMP_Text textoDinero;

    private void Start()
    {
        ActualizarUI();
    }

    public void SumarDinero(int cantidad)
    {
        dinero += cantidad;
        ActualizarUI();
    }

    public void RestarDinero(int cantidad)
    {
        Debug.Log($"[PlayerDinero] Se restan ${cantidad} — Total antes: {dinero}");
        dinero -= cantidad;
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        if (textoDinero != null)
            textoDinero.text = "$" + dinero.ToString();
    }
}


