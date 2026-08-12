using UnityEngine;

public class PickupPart : MonoBehaviour, IInteractable
{
    [Header("Part Settings")]
    [SerializeField] private string partId = "Part_01_Battery";
    [SerializeField] private string displayName = "Battery";

    [Header("Optional Effects")]
    [SerializeField] private AudioSource pickupAudio;
    [SerializeField] private GameObject visualObject;

    private bool collected;

    public string GetInteractionPrompt()
    {
        if (collected)
            return string.Empty;

        return $"Press E to collect {displayName}";
    }

    public void Interact(GameObject interactor)
    {
        if (collected)
            return;

        if (GameManager.Instance == null)
        {
            Debug.LogError("No GameManager exists in the scene.");
            return;
        }

        bool registered = GameManager.Instance.RegisterPart(partId);

        if (!registered)
            return;

        collected = true;

        if (pickupAudio != null)
            pickupAudio.Play();

        if (visualObject != null)
            visualObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}