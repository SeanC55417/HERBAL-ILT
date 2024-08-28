using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IncubatorScript : MonoBehaviour
{
    // Public references
    public GameObject trypsinObject; // Reference to the Trypsin GameObject
    public GameObject mediumObject;  // Reference to the Medium GameObject
    public MttAssaySceneScript sceneController; // Reference to the scene script for setting state
    public Image heatingProgressBar; // UI Image component for heating progress
    public Button doorButton; // Reference to the Button component on the door GameObject

    // Private variables to track state
    private bool isTrypsinInside = false;
    private bool isMediumInside = false;
    private readonly float heatingDelay = 3f; // Time delay for heating

    void OnTriggerEnter(Collider other)
    {
        // Check if Trypsin or Medium has entered the incubator
        if (other.gameObject == trypsinObject)
        {
            isTrypsinInside = true;
        }
        else if (other.gameObject == mediumObject)
        {
            isMediumInside = true;
        }

        UpdateIncubatorState();
    }

    void OnTriggerExit(Collider other)
    {
        // Check if Trypsin or Medium has exited the incubator
        if (other.gameObject == trypsinObject)
        {
            isTrypsinInside = false;
        }
        else if (other.gameObject == mediumObject)
        {
            isMediumInside = false;
        }

        UpdateIncubatorState();
    }

    // Updates the scene script based on whether both Trypsin and Medium are inside
    private void UpdateIncubatorState()
    {
        bool areBothInside = isTrypsinInside && isMediumInside;
        sceneController.SetTrypsinAndMediumInIncubator(areBothInside);
    }

    // Locks the door by disabling the Button component and starts the heating process
    public void StartHeatingProcess()
    {
        DisableDoorButton(); // Disable the door button to prevent interaction
        StartCoroutine(HeatIncubator()); // Begin heating coroutine
    }

    // Coroutine to handle heating logic with a delay and update the progress bar
    private IEnumerator HeatIncubator()
    {
        float elapsedTime = 0f;
        heatingProgressBar.fillAmount = 0f; // Reset progress bar at the start

        while (elapsedTime < heatingDelay)
        {
            elapsedTime += Time.deltaTime;
            heatingProgressBar.fillAmount = Mathf.Clamp01(elapsedTime / heatingDelay); // Update fill amount based on elapsed time
            yield return null; // Wait for the next frame
        }

        heatingProgressBar.fillAmount = 0f; // Ensure progress bar is full after heating
        EnableDoorButton(); // Enable the door button after heating
    }

    // Disables the Button component on the door
    private void DisableDoorButton()
    {
        doorButton.interactable = false;
    }

    // Enables the Button component on the door
    private void EnableDoorButton()
    {
        doorButton.interactable = true;
    }
}
