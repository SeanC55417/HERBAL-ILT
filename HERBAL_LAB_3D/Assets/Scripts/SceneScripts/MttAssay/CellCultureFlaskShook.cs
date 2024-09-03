using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCultureFlaskShook : MonoBehaviour
{
    public float shakeThreshold = 1.0f; // The total distance that must be accumulated for the object to be considered shaken
    public float movementSensitivity = 0.1f; // The minimum movement in any direction to be considered part of the shake
    public bool isShakeDetectionActive = false; // Variable to start/stop the shake detection
    private Vector3 lastPosition; // The position of the object in the last frame
    private float accumulatedDistance = 0.0f; // The accumulated movement distance
    private bool isShaken = false; // To store if the object has been shaken

    // Start is called before the first frame update
    void Start()
    {
        // Initialize lastPosition to the object's starting position
        lastPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (isShakeDetectionActive)
        {
            // Calculate the distance moved since the last frame
            Vector3 movement = transform.position - lastPosition;

            // Accumulate the distance if it exceeds the sensitivity threshold
            if (movement.magnitude > movementSensitivity)
            {
                accumulatedDistance += movement.magnitude;
            }

            // Update the last position for the next frame
            lastPosition = transform.position;

            // Check if the accumulated distance exceeds the shake threshold
            if (accumulatedDistance >= shakeThreshold)
            {
                isShaken = true;
                // Debug.Log("The object has been shaken!");
                // Stop the shake detection after the object is shaken
                isShakeDetectionActive = false;
            }
        }
    }

    // Method to start shake detection
    public void StartShakeDetection()
    {
        isShakeDetectionActive = true;
        accumulatedDistance = 0.0f; // Reset the accumulated distance
        isShaken = false; // Reset the shaken state
        lastPosition = transform.position; // Reset the last position
    }

    // Method to stop shake detection
    public void StopShakeDetection()
    {
        isShakeDetectionActive = false;
    }

    // Getter method to report if the object has been shaken
    public bool IsShaken()
    {
        return isShaken;
    }
}
