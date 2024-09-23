using UnityEngine;
using System.Collections;

public class ShowHideCycle : MonoBehaviour
{
    public Renderer objectRenderer; // Assign the Renderer component in the Inspector
    public float hideDuration = 3f; // Duration to keep the object hidden
    public float showDuration = 5f; // Duration to keep the object visible

    public void StartVisibilityCycle()
    {
        StartCoroutine(VisibilityCycle());
    }

    private IEnumerator VisibilityCycle()
    {
        while (true)
        {
            // Hide the object
            objectRenderer.enabled = false;
            yield return new WaitForSeconds(hideDuration);

            // Show the object
            objectRenderer.enabled = true;
            yield return new WaitForSeconds(showDuration);

            // Hide the object again
            objectRenderer.enabled = false;
            yield break; // End the cycle after hiding the object
        }
    }
}
