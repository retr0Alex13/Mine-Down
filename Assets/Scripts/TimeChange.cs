using System.Collections;
using UnityEngine;

public class TimeChange : MonoBehaviour
{
    [SerializeField] private float targetTimeScale = 0.2f; // Use 'targetTimeScale' for clarity
    [SerializeField] private float slowdownSpeed = 0.1f; // Adjustable slowdown rate
    [SerializeField] private bool smoothTransition; // Control whether to use smoothing

    private Coroutine coroutine; // Reference to active coroutine

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += BeginSlowdown;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= BeginSlowdown;
        StopCoroutineIfRunning(); // Ensure coroutine is stopped on disable
    }

    private void BeginSlowdown()
    {
        StopCoroutineIfRunning(); // Stop potential previous coroutines
        if (smoothTransition)
        {
            coroutine = StartCoroutine(SmoothSlowdown());
        }
        else
        {
            Time.timeScale = targetTimeScale; // Immediate slowdown if smoothness is disabled
        }
    }

    private IEnumerator SmoothSlowdown()
    {
        float startScale = Time.timeScale; // Store starting time scale for smoother transition
        float elapsedTime = 0f;

        while (elapsedTime < slowdownSpeed)
        {
            Time.timeScale = Mathf.Lerp(startScale, targetTimeScale, elapsedTime / slowdownSpeed);
            elapsedTime += Time.deltaTime; // Use Time.deltaTime for frame-rate independence
            yield return null;
        }

        Time.timeScale = targetTimeScale; // Ensure final target value is reached
    }

    private void StopCoroutineIfRunning()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
