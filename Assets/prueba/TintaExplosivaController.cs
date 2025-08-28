using UnityEngine;
using UnityEngine.UI;

public class TintaExplosivaController : MonoBehaviour
{
    public Image tintaExplosivaImage;
    private bool isActive = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            isActive = !isActive; // alterna true/false
            tintaExplosivaImage.gameObject.SetActive(isActive);
        }
    }
}
