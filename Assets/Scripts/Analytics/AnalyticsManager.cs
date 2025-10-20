using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using System.Threading.Tasks;


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance;

    private async Task Awake()
    {
        DontDestroyOnLoad(gameObject);

        await InitializeAnalytics();
    }
    private async Task InitializeAnalytics()
    {
        try
        {
            // Inicializa todos los servicios de Unity (Analytics, Cloud Save, etc.)
            await UnityServices.InitializeAsync();

            // Comienza la recolección de datos de Analytics
            AnalyticsService.Instance.StartDataCollection();

            Debug.Log("✅ Unity Analytics inicializado correctamente");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Error al inicializar Unity Analytics: {e.Message}");
        }
    }
}
