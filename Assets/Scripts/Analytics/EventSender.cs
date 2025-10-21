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

    public static void SendFinish(int value, string name)
    {
        var parameters = new Dictionary<string, object>
        {
            { "Finish", value },
            { "FinishLvlName", name },

        };

        AnalyticsService.Instance.CustomData("Finish", parameters);
       
        AnalyticsService.Instance.Flush();
        Debug.Log("Event sent: Finish");
    }

    public static void SendLevelQuit(int value, string name)
    {
        var parameters = new Dictionary<string, object>
        {
            { "LevelQuit", value },
            { "LevelQuitName", name },

        };

        AnalyticsService.Instance.CustomData("LevelQuit", parameters);
        
        AnalyticsService.Instance.Flush();
        Debug.Log("Event sent: LevelQuit");
    }

    public static void SendLevelStart(int value, string name)
    {
        var parameters = new Dictionary<string, object>
        {
            { "LevelStart", value },
            { "LevelStartName", name },

        };

        AnalyticsService.Instance.CustomData("LevelStart", parameters);
        
        AnalyticsService.Instance.Flush();
        Debug.Log("Event sent: levelStart");
    }

    public static void SendEnemiesKilled(int value,string name)
    {
        name = name.Length >= 7 ? name.Substring(0, 7) : name;

        var parameters = new Dictionary<string, object>
        {
            { "EnemiesKilled", value },
            { "EnemiesKilledName", name },

        };

        
        AnalyticsService.Instance.CustomData("EnemiesKilled", parameters);
        AnalyticsService.Instance.Flush();
        Debug.Log("Event sent: EnemiesKilledName");
    }


}
