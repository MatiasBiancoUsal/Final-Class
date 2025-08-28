using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ProjectileSpeedModifier : MonoBehaviour
{
    [Tooltip("Proyectiles Enemigos")]
    public MonoBehaviour enemyScript;

    [Tooltip("Nombre de la variable dentro del script que controla la velocidad")]
    public string projectileSpeedVariableName = "projectileSpeed";

    private bool boosted = false;

    // Guardamos los valores originales de velocidad de cada enemigo
    private Dictionary<MonoBehaviour, float> originalSpeeds = new Dictionary<MonoBehaviour, float>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            System.Type scriptType = enemyScript.GetType();
            MonoBehaviour[] enemies = FindObjectsOfType(scriptType) as MonoBehaviour[];

            foreach (MonoBehaviour enemy in enemies)
            {
                FieldInfo speedField = scriptType.GetField(projectileSpeedVariableName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (speedField != null)
                {
                    float currentValue = (float)speedField.GetValue(enemy);

                    if (!boosted)
                    {
                        // Guardamos el valor original
                        if (!originalSpeeds.ContainsKey(enemy))
                            originalSpeeds.Add(enemy, currentValue);

                        speedField.SetValue(enemy, currentValue * 1.4f); // +40%
                    }
                    else
                    {
                        // Restauramos el valor original
                        if (originalSpeeds.ContainsKey(enemy))
                            speedField.SetValue(enemy, originalSpeeds[enemy]);
                    }
                }
            }

            boosted = !boosted; // Cambiamos el estado: true -> false o false -> true
        }
    }
}
