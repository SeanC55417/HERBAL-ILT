using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public Canvas mainMenu;      // Reference to the main menu canvas
    public Canvas moduleSection; // Reference to the module section canvas
    public Canvas about;
    public Canvas settings;      // Reference to the settings canvas

    private Canvas currentCanvas; // Track the current canvas that is active

    void Start()
    {
        // Initialize the current canvas
        currentCanvas = mainMenu;  // Assuming mainMenu is the default canvas to be shown initially
        ShowCanvas(currentCanvas);
    }

    // Method to show a specific canvas
    public void ShowCanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(true);
        currentCanvas = canvas;  // Update the current canvas to the new one
    }

    // Method to hide a specific canvas
    public void HideCanvas(Canvas canvas)
    {
        canvas.gameObject.SetActive(false);
    }

    // Method to change to a specified canvas
    public void ChangeCanvasTo(Canvas newCanvas)
    {
        if(currentCanvas != null)
            HideCanvas(currentCanvas); // Hide the currently active canvas

        ShowCanvas(newCanvas); // Show the new canvas and update the current canvas
    }
}
