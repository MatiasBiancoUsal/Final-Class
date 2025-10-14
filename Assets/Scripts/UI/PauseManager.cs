using UnityEngine;
using Unity.Services.Analytics;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    [SerializeField] private GameObject pauseMenuRoot; // assign your menu panel

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    private void Awake()
    {
        IsPaused = false;
        Time.timeScale = 1f;

        if (pauseMenuRoot) pauseMenuRoot.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        Time.timeScale = IsPaused ? 0f : 1f;

        if (pauseMenuRoot) pauseMenuRoot.SetActive(IsPaused);

        Cursor.lockState = IsPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible   = IsPaused;
    }

    // optional UI buttons
    public void Resume() { if (IsPaused) TogglePause(); }
    public void QuitToMenu() { Time.timeScale = 1f; /* SceneManager.LoadScene(...); */ }
}
