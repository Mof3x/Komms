using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    [Header("Pickup")]
    public AudioClip pickupSound;

    // Optional: how loud the sound should be
    [Range(0f, 1f)]
    public float pickupVolume = 1f;

    private AudioSource playerAudioSource;

    void Start()
    {
        // Find the player's AudioSource in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAudioSource = player.GetComponent<AudioSource>();
        }

        if (playerAudioSource == null)
        {
            Debug.LogWarning("CollectibleItem: No AudioSource found on Player. Pickup sounds will be silent.");
        }
    }

    // Use trigger collision for simple pickups:
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayPickupSound();
            Collect();
        }
    }

    void PlayPickupSound()
    {
        if (playerAudioSource != null && pickupSound != null)
        {
            playerAudioSource.PlayOneShot(pickupSound, pickupVolume);
        }
    }

    void Collect()
    {
        // Your existing collect logic goes here:
        // e.g. increment parts counter, update UI, etc.
        // Example:
        // GameManager.Instance.AddPart();

        Destroy(gameObject);
    }
}