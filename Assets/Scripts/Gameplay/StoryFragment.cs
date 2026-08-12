using UnityEngine;

public class StoryFragment : MonoBehaviour, IInteractable
{
    [Header("Story Content")]
    [SerializeField] private string fragmentTitle = "Community note";
    [TextArea(3, 8)]
    [SerializeField] private string fragmentText =
        "The radio station keeps the community connected.";

    [Header("Interaction")]
    [SerializeField] private bool canReadOnlyOnce = true;

    private bool hasBeenRead;

    public string GetInteractionPrompt()
    {
        if (hasBeenRead && canReadOnlyOnce)
            return string.Empty;

        return $"Press E to read {fragmentTitle}";
    }

    public void Interact(GameObject interactor)
    {
        if (hasBeenRead && canReadOnlyOnce)
            return;

        HUDController hud =
            FindFirstObjectByType<HUDController>();

        if (hud != null)
            hud.ShowStoryMessage(fragmentText);

        hasBeenRead = true;

        if (canReadOnlyOnce)
            gameObject.SetActive(false);
    }
}