using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public static class EventSender 
{
    public static void SendDeathPlayer(int value)
    {
        var parameters = new Dictionary<string, object>
        {
            { "DeathPlayer", value },
            
        };

        AnalyticsService.Instance.CustomData("DeathPlayer", parameters);
        AnalyticsService.Instance.Flush(); 
        Debug.Log("Event sent: DeathPlayer");
    }
}
