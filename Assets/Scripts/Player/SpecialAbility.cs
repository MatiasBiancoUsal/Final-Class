using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpecialAbility : MonoBehaviour
{
    public enum CharacterType
    {
        Default,    // Personaje inicial sin habilidad
    }

    public CharacterType characterType = CharacterType.Default;

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
                print("Habilidad especial no disponible.");
                break;
        }
    }
}

