using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TMP_Text objectiveText;
    [SerializeField] private TMP_Text partsText;
    [SerializeField] private TMP_Text interactionPrompt;
    [SerializeField] private TMP_Text batteryPercentText;
    [SerializeField] private TMP_Text spottedText;
    [SerializeField] private TMP_Text storyText;

    [Header("Battery")]
    [SerializeField] private Slider batterySlider;

    [Header("End Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject failurePanel;

    [Header("Message Timing")]
    [SerializeField] private float storyMessageDuration = 4f;

    private Coroutine storyCoroutine;

    private void OnEnable()
    {
        Debug.Log("HUDController OnEnable");

        StartCoroutine(SubscribeNextFrame());
    }

    private IEnumerator SubscribeNextFrame()
    {
        yield return null;

        if (GameManager.Instance != null)
        {
            Debug.Log("HUDController subscribed to GameManager");
            GameManager.Instance.PartsChanged += UpdateParts;
            GameManager.Instance.StateChanged += UpdateGameState;
            GameManager.Instance.SpottedChanged += SetSpotted;
        }
        else
        {
            Debug.LogWarning("HUDController: GameManager.Instance still null");
        }

        BatteryManager batteryManager = FindFirstObjectByType<BatteryManager>();

        if (batteryManager != null)
            batteryManager.BatteryChanged += UpdateBattery;

        RefreshInitialState();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.PartsChanged -= UpdateParts;
            GameManager.Instance.StateChanged -= UpdateGameState;
            GameManager.Instance.SpottedChanged -= SetSpotted;
        }

        BatteryManager batteryManager = FindFirstObjectByType<BatteryManager>();

        if (batteryManager != null)
        {
            Debug.Log("HUDController subscribed to BatteryManager");
            batteryManager.BatteryChanged -= UpdateBattery;
        }
    }

    private void RefreshInitialState()
    {
        if (objectiveText != null)
            objectiveText.text = "Find 3 transmitter components";

        if (interactionPrompt != null)
            interactionPrompt.text = string.Empty;

        if (spottedText != null)
            spottedText.gameObject.SetActive(false);

        if (storyText != null)
            storyText.gameObject.SetActive(false);

        if (winPanel != null)
            winPanel.SetActive(false);

        if (failurePanel != null)
            failurePanel.SetActive(false);

        if (GameManager.Instance != null)
        {
            UpdateParts(
                GameManager.Instance.CollectedParts,
                GameManager.Instance.TotalParts
            );

            UpdateGameState(GameManager.Instance.CurrentState);
        }

        BatteryManager batteryManager = FindFirstObjectByType<BatteryManager>();

        if (batteryManager != null)
            UpdateBattery(batteryManager.CurrentBattery, batteryManager.MaxBattery);
    }

    private void UpdateParts(int collected, int total)
    {
        Debug.Log("HUDController UpdateParts: " + collected + "/" + total);

        if (partsText != null)
            partsText.text = $"Parts: {collected}/{total}";
    }

    private void UpdateBattery(float current, float maximum)
    {
        if (batterySlider != null)
        {
            batterySlider.maxValue = maximum;
            batterySlider.value = current;
        }

        if (batteryPercentText != null)
        {
            float percentage = maximum <= 0f
                ? 0f
                : current / maximum * 100f;

            batteryPercentText.text = $"Battery: {Mathf.CeilToInt(percentage)}%";
        }
    }

    private void UpdateGameState(GameState state)
    {
        Debug.Log("HUDController UpdateGameState: " + state);

        if (winPanel != null)
            winPanel.SetActive(state == GameState.Won);

        if (failurePanel != null)
            failurePanel.SetActive(state == GameState.Failed);

        if (state != GameState.Playing)
            ClearInteractionPrompt();
    }

    public void SetInteractionPrompt(string message)
    {
        if (interactionPrompt != null)
            interactionPrompt.text = message;
    }

    public void ClearInteractionPrompt()
    {
        if (interactionPrompt != null)
            interactionPrompt.text = string.Empty;
    }

    public void SetSpotted(bool spotted)
    {
        if (spottedText != null)
            spottedText.gameObject.SetActive(spotted);
    }

    public void ShowStoryMessage(string message)
    {
        if (storyCoroutine != null)
            StopCoroutine(storyCoroutine);

        storyCoroutine = StartCoroutine(StoryMessageRoutine(message));
    }

    private IEnumerator StoryMessageRoutine(string message)
    {
        if (storyText != null)
        {
            storyText.text = message;
            storyText.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(storyMessageDuration);

        if (storyText != null)
            storyText.gameObject.SetActive(false);
    }
}