using UnityEngine;

public class BombibumAnimationEvents : MonoBehaviour
{
    [Tooltip("Referencia al BombibumHealth en el parent")]
    public BombibumHealth health;

    public void OnDeathEnd()
    {
        if (health != null)
            health.OnDeathAnimationEnd();
    }
}
