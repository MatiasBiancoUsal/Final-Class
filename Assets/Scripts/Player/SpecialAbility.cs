using System.Collections;
using UnityEngine;
using TMPro;

public class SpecialAbility : MonoBehaviour
{
    public enum CharacterType
    {
        Default,    // Personaje inicial sin habilidad
    }

    public CharacterType characterType = CharacterType.Default;

    [Header("Referencia al mensaje en pantalla")]
    public TextMeshProUGUI mensajeUI;

    [Header("Duración del mensaje en pantalla")]
    public float duracionMensaje = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpecial();
        }
    }

    void ActivateSpecial()
    {
        switch (characterType)
        {
            case CharacterType.Default:
                Debug.Log("Este personaje no tiene habilidad especial.");
                MostrarMensaje("Este personaje no tiene habilidad especial.");
                break;
        }
    }

    void MostrarMensaje(string texto)
    {
        if (mensajeUI != null)
        {
            mensajeUI.text = texto;
            mensajeUI.gameObject.SetActive(true);
            CancelInvoke(nameof(EsconderMensaje));
            Invoke(nameof(EsconderMensaje), duracionMensaje);
        }
    }

    void EsconderMensaje()
    {
        if (mensajeUI != null)
            mensajeUI.gameObject.SetActive(false);
    }
}


