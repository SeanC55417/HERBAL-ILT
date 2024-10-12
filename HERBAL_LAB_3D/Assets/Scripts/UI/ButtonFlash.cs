using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFlash : MonoBehaviour
{
    // Reference to the button
    public Color flashColor = Color.yellow; // Color to flash
    public float flashDuration = .5f; // Duration of each flash in seconds

    private Color originalColor;
    private Image buttonImage;
    private Coroutine flashCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>(); // Get the Button component from the current GameObject
        if (button != null)
        {
            buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                originalColor = buttonImage.color;
            }
        }
    }

    // Method to start the flashing effect
    public void StartFlashing()
    {
        if (flashCoroutine == null && buttonImage != null)
        {
            flashCoroutine = StartCoroutine(FlashButton());
        }
    }

    // Method to stop the flashing effect
    public void StopFlashing()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
            if (buttonImage != null)
            {
                buttonImage.color = originalColor;
            }
        }
    }

    private IEnumerator FlashButton()
    {
        while (true)
        {
            buttonImage.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            buttonImage.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}
