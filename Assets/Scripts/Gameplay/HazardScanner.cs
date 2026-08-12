using System.Collections;
using UnityEngine;

public class HazardScanner : MonoBehaviour
{
    [Header("Hazard Settings")]
    [SerializeField] private float warningDuration = 3f;
    [SerializeField] private bool failPlayer = false;

    private Coroutine warningCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance == null)
            return;

        GameManager.Instance.SetSpotted(true);

        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(
            WarningRoutine()
        );
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (warningCoroutine != null)
            StopCoroutine(warningCoroutine);

        warningCoroutine = StartCoroutine(
            ClearWarningRoutine()
        );
    }

    private IEnumerator WarningRoutine()
    {
        yield return new WaitForSeconds(warningDuration);

        if (failPlayer &&
            GameManager.Instance != null &&
            GameManager.Instance.CurrentState == GameState.Playing)
        {
            GameManager.Instance.FailGame();
        }
    }

    private IEnumerator ClearWarningRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        if (GameManager.Instance != null)
            GameManager.Instance.SetSpotted(false);
    }
}
