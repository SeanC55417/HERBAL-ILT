using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimationOnStart : MonoBehaviour
{
    public GameObject[] buttons;       // Array of buttons to animate
    public float horizontalStart = -100f;  // Starting x-position (off-screen)
    public float horizontalEnd = 0f;    // Ending x-position (final position)
    public float durationSec = 2f;      // Duration of the animation in seconds
    public float staggerDelay = 0.5f;   // Delay between starting animations of consecutive buttons

    private int animationsRemaining;    // Counter to track remaining animations

    // Coroutine to fade in and slide a button with delay
    IEnumerator FadeInAndSlide(GameObject button, float delay)
    {
        // Initial delay before starting the animation
        yield return new WaitForSeconds(delay);

        float timeElapsed = 0;
        CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
        RectTransform rect = button.GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = button.AddComponent<CanvasGroup>();  // Add CanvasGroup if not present

        Vector3 startPosition = new Vector3(horizontalStart, rect.localPosition.y, rect.localPosition.z);
        Vector3 endPosition = new Vector3(horizontalEnd, rect.localPosition.y, rect.localPosition.z);

        // Animation loop
        while (timeElapsed < durationSec)
        {
            // Calculate the fraction of the duration that has passed
            float t = timeElapsed / durationSec;
            canvasGroup.alpha = Mathf.Lerp(0, 1, t);  // Lerp alpha from 0 to 1
            rect.localPosition = Vector3.Lerp(startPosition, endPosition, t);  // Lerp position from start to end

            timeElapsed += Time.deltaTime;  // Increase the time elapsed
            yield return null;  // Wait until next frame
        }

        // Ensure final values are set
        canvasGroup.alpha = 1;
        rect.localPosition = endPosition;

        // Decrease the counter when this animation is done
        animationsRemaining--;
        if (animationsRemaining == 0)
        {
            EnableAllButtons();
        }
    }

    // Enable interaction for all buttons
    void EnableAllButtons()
    {
        foreach (GameObject button in buttons)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.interactable = true;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animationsRemaining = buttons.Length; // Set the counter to the number of buttons

        // Start the animation for each button with staggered delays
        float currentDelay = 0;
        foreach (GameObject button in buttons)
        {
            CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = button.AddComponent<CanvasGroup>();  // Add CanvasGroup if not present
            }
            canvasGroup.alpha = 0;  // Set initial alpha to 0
            canvasGroup.interactable = false;  // Initially disable interaction
            StartCoroutine(FadeInAndSlide(button, currentDelay));
            currentDelay += staggerDelay;  // Increment the delay for the next button
        }
    }
}
