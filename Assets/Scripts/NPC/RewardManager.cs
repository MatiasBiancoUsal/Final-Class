using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    public int playerPoints = 0;

    public void GiveReward(int points)
    {
        playerPoints += points;
        Debug.Log("¡Ganaste " + points + " puntos! Total: " + playerPoints);
    }
}
