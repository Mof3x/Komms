using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Main Buttons Group")]
    [SerializeField] private GameObject mainButtonsGroup;   // parent holding Play/Controls/Quit

    [Header("Controls Panel")]
    [SerializeField] private GameObject controlsPanel;      // the overlay panel

    private void Start()
    {
        ShowMainMenu();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Komms_VerticalSlice");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowControls()
    {
        if (mainButtonsGroup != null)
            mainButtonsGroup.SetActive(false);

        if (controlsPanel != null)
            controlsPanel.SetActive(true);
    }

    public void HideControls()
    {
        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        if (mainButtonsGroup != null)
            mainButtonsGroup.SetActive(true);

        if (controlsPanel != null)
            controlsPanel.SetActive(false);
    }
}