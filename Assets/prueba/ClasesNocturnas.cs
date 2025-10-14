using UnityEngine;
using Unity.Services.Analytics;

public class ClasesNocturnas : MonoBehaviour
{
    public GameObject visionSphere; 
    private bool isActive = false;

    void Start()
    {
        visionSphere.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            isActive = !isActive;
            visionSphere.SetActive(isActive);
        }
    }
}
