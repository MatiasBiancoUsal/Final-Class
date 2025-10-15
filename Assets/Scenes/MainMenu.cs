using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Analytics;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject charactersPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Prueba");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OpenCharacters()
    {
        charactersPanel.SetActive(true);
    }

    public void ClosePanels()
    {
        settingsPanel.SetActive(false);
        charactersPanel.SetActive(false);
    }
}

