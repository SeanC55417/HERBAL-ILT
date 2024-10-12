using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDrawXAxis : MonoBehaviour
{
    public Vector3 openPosition;
    public float transitionDuration = 1.0f;
    
    private Vector3 startingPosition;
    private bool isTransitioning = false;
    private bool isOpen = false;
    private float transitionProgress = 0.0f;

    void Start()
    {
        // Initialize the drawer to the closed position
        // transform.localPosition = startingPosition;
        // Store the initial position and rotation
        startingPosition = transform.localPosition;
    }

    void FixedUpdate()
    {
        if (isTransitioning)
        {
            // Increment the transition progress
            transitionProgress += Time.deltaTime / transitionDuration;

            // Interpolate the drawer's position
            if (isOpen)
            {
                transform.localPosition = Vector3.Lerp(openPosition, startingPosition, transitionProgress);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startingPosition, openPosition, transitionProgress);
            }

            // Stop transitioning when the drawer is fully open or closed
            if (transitionProgress >= 1.0f)
            {
                isTransitioning = false;
                transitionProgress = 0.0f;
                isOpen = !isOpen;
            }
        }
    }

    // Call this method to toggle the drawer's state
    public void ToggleDrawer()
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            transitionProgress = 0.0f;
        }
    }
}
