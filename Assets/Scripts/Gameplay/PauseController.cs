using UnityEngine;

public class PauseController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject pausePanel;

    private bool isPaused;

    private void Awake()
    {
        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;

        // Start locked for FPS look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // P → pause (only if not already paused)
        if (!isPaused && Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
        // (You can keep resume via button only)
    }

    public void Pause()
    {
        isPaused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;

        // Unlock cursor so you can click UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Called by Resume button
    public void Resume()
    {
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;

        // Lock cursor again for FPS look
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool IsPaused => isPaused;
}