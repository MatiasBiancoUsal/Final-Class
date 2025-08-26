using UnityEngine;

public class SpeedModifier : MonoBehaviour
{
    [Header("Recreo Cancelado")]
    public PlayerController playerController;

    private float originalSpeed;
    private bool speedReduced = false;

    void Start()
    {
        if (playerController != null)
        {
            originalSpeed = playerController.moveSpeed;
        }
    }

    void Update()
    {
        if (playerController == null) return;

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!speedReduced)
            {
                playerController.moveSpeed *= 0.75f; // baja 25%
                speedReduced = true;
            }
            else
            {
                playerController.moveSpeed = originalSpeed; // vuelve al original
                speedReduced = false;
            }
        }
    }
}
