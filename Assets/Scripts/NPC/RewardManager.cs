using UnityEngine;
using Unity.Services.Analytics;
public class RewardManager : MonoBehaviour
{
    public int coins;
    public void GiveReward(int amount)
    {
        coins += Mathf.Max(0, amount);
        Debug.Log($"Coins: {coins}");
    }
}
