using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI; // Add this to use UI components

public class Vortex : MonoBehaviour
{
    public GameObject tube; // Reference to the tube GameObject
    public GameObject tube2;
    public float requiredContactTime = 3f; // The total time the tube needs to be in contact with the vortex

    private float contactTime = 0f; // To track the cumulative contact time
    private bool isTubeInContact = false; // To track if the tube is currently in contact
    public bool vortexed = false;
    public UnityEngine.UI.Image progress; // Reference to the UI Image component for the progress circle

    void Update()
    {
        // Increment the contact time if the tube is in contact
        if (isTubeInContact)
        {
            contactTime += Time.deltaTime;

            // Update the progress circle fill amount based on contact time
            progress.fillAmount = contactTime / requiredContactTime;

            // Check if the required contact time has been reached
            if (contactTime >= requiredContactTime)
            {
                OnContactTimeReached();
                // Reset the contact time so the event doesn't trigger again immediately
                contactTime = 0f;
                isTubeInContact = false; // Stop further counting until the tube exits and re-enters
                vortexed = true;

                // Reset the progress fill amount to 0
                progress.fillAmount = 0f;
            }
        }
        else
        {
            // If the tube is not in contact, reset the progress fill
            progress.fillAmount = 0f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the tube has entered the collider
        if (other.gameObject == tube || other.gameObject == tube2)
        {
            isTubeInContact = true;
            Debug.Log("Tube entered the vortex.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the tube has exited the collider
        if (other.gameObject == tube || other.gameObject == tube2)
        {
            isTubeInContact = false;
            Debug.Log("Tube exited the vortex.");
            
            // Reset the progress fill amount when the tube exits
            progress.fillAmount = 0f;
        }
    }

    private void OnContactTimeReached()
    {
        // This method is called when the tube has been in contact with the vortex for the required amount of time
        Debug.Log("Tube has been in contact with the vortex for the required time.");
        // Add your custom logic here for what happens when the tube has been in contact long enough
    }
}
