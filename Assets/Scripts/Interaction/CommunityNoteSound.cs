using UnityEngine;

public class CommunityNote : MonoBehaviour, IInteractable
{
    [Header("Audio")]
    public AudioClip readNoteSound;
    [Range(0f, 1f)] public float readNoteVolume = 1f;

    private AudioSource playerAudioSource;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();
        }
    }

    public string GetInteractionPrompt()
    {
        return "Press E to read note";
    }

    public void Interact(GameObject interactor)
    {
        PlayReadSound();

        // Your existing note UI logic:
        // NoteUI.Instance.ShowNote(noteText);
    }

    private void PlayReadSound()
    {
        if (playerAudioSource != null && readNoteSound != null)
        {
            playerAudioSource.PlayOneShot(readNoteSound, readNoteVolume);
        }
    }
}