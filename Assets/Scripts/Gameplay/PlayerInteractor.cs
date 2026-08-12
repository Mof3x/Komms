using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers = ~0; 

    private IInteractable currentInteractable;
    private HUDController hud;

    private void Awake()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        hud = FindFirstObjectByType<HUDController>();
        Debug.Log("PlayerInteractor HUD = " + hud);
    }

    private void Update()
    {
        FindInteractable();

        if (currentInteractable != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentInteractable.Interact(gameObject);
        }
    }

    private void FindInteractable()
    {
        IInteractable detectedInteractable = null;

        if (playerCamera != null)
        {
            Ray ray = new Ray(
                playerCamera.transform.position,
                playerCamera.transform.forward
            );

            if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                interactionDistance,
                interactionLayers,
                QueryTriggerInteraction.Collide))
            {
                MonoBehaviour[] behaviours =
                    hit.collider.GetComponentsInParent<MonoBehaviour>();

                foreach (MonoBehaviour behaviour in behaviours)
                {
                    if (behaviour is IInteractable interactable)
                    {
                        detectedInteractable = interactable;
                        break;
                    }
                }
            }
        }

        currentInteractable = detectedInteractable;

        if (hud == null)
        {
            Debug.LogWarning("PlayerInteractor: HUDController reference is null");
            return;
        }

        if (currentInteractable != null)
        {
            string prompt = currentInteractable.GetInteractionPrompt();
            hud.SetInteractionPrompt(prompt);
        }
        else
        {
            hud.ClearInteractionPrompt();
        }
    }
}