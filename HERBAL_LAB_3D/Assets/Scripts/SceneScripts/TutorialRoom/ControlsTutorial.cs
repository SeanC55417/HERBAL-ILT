using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsTutorial : MonoBehaviour
{
    public TutorialManager tutorialManager; // Reference to the TutorialManager to notify when task is complete
    private bool hasMoved = false;          // Flag to check if movement has occurred

    void Update()
    {
        // Check for basic movement input from WASD or arrow keys
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            if (!hasMoved) // Check if this is the first movement
            {
                hasMoved = true; // Set the flag so this block won't run again
                CompleteMovementTask(); // Notify that the movement task is complete
            }
        }
    }

    private void CompleteMovementTask()
    {
        if (tutorialManager != null)
        {
            tutorialManager.AdvanceTutorial(); // Call the method to advance the tutorial
        }
        else
        {
            Debug.LogError("TutorialManager reference not set in TutorialMovement.");
        }
    }
}
