using UnityEngine;

public class TransmitterRepair : MonoBehaviour, IInteractable
{
    [Header("Repair Settings")]
    [SerializeField] private GameObject repairedVisual;
    [SerializeField] private GameObject unrepairedVisual;

    [Header("Optional Audio")]
    [SerializeField] private AudioSource repairAudio;
[SerializeField] private AudioSource broadcastAudio;

    private bool repaired;

    public string GetInteractionPrompt()
    {
        if (repaired)
            return string.Empty;

        if (GameManager.Instance == null)
            return "Transmitter unavailable";

        if (!GameManager.Instance.HasAllParts)
        {
            int remaining =
                GameManager.Instance.TotalParts -
                GameManager.Instance.CollectedParts;

            return $"Need {remaining} more component(s)";
        }

        return "Press E to repair transmitter";
    }

    public void Interact(GameObject interactor)
    {
        if (repaired)
            return;

        if (GameManager.Instance == null)
            return;

        if (!GameManager.Instance.HasAllParts)
        {
            Debug.Log("You need all three components first.");
            return;
        }

        repaired = true;

        if (repairAudio != null)
            repairAudio.Play();

        if (unrepairedVisual != null)
            unrepairedVisual.SetActive(false);

        if (repairedVisual != null)
            repairedVisual.SetActive(true);

        GameManager.Instance.WinGame();

        if (broadcastAudio != null)
        {
            broadcastAudio.PlayDelayed(0.5f);
        }
    }
}