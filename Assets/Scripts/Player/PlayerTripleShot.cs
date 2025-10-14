using UnityEngine;
using Unity.Services.Analytics;

public class PlayerTripleShot : MonoBehaviour
{
    public bool tripleShotActive = false;

    public void EnableTripleShot(float duration)
    {
        tripleShotActive = true;
        CancelInvoke(nameof(DisableTripleShot));
        Invoke(nameof(DisableTripleShot), duration);
    }

    private void DisableTripleShot()
    {
        tripleShotActive = false;
    }
}
