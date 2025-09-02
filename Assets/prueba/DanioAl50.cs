using UnityEngine;

[AddComponentMenu("Combat/Daño al 50%")]
public class DanioAl50 : MonoBehaviour
{
    [Range(1f, 3f)] public float multiplier = 1.5f; // +50%
    public bool active = false;                      

    
    public float Current => active ? multiplier : 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            active = !active;                        // alterna con B
    }
}
