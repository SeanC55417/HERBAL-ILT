using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI components

public class Vortex : MonoBehaviour
{
    [Header("Tube References")]
    public GameObject tube;  // Reference to the first tube GameObject

    [Header("Vortex Settings")]
    public float requiredContactTime = 3f; // The total time the tube needs to be in contact with the vortex

    [Header("UI References")]
    public Image progress; // Reference to the UI Image component for the progress circle

    private float contactTime = 0f; // To track the cumulative contact time
    private bool isTubeInContact = false; // To track if the tube is currently in contact
    private bool vortexed = false; // To track if the vortex effect has been triggered
    private bool vortexActive = false; // To control whether the vortex effect is active

    // Property to get and set the vortex active state
    public bool VortexActive
    {
        get { return vortexActive; }
        set { vortexActive = value; }
    }

    // Property to get and set the vortexed state
    public bool Vortexed
    {
        get { return vortexed; }
        set { vortexed = value; }
    }

    private void Update()
    {
        if (vortexActive)
        {
            VortexContact();
        }
    }

    private void VortexContact()
    {
        if (isTubeInContact)
        {
            contactTime += Time.deltaTime;
            progress.fillAmount = contactTime / requiredContactTime;

            if (contactTime >= requiredContactTime)
            {
                OnContactTimeReached();
            }
        }
        else
        {
            progress.fillAmount = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == tube)
        {
            isTubeInContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == tube)
        {
            isTubeInContact = false;
            progress.fillAmount = 0f;
        }
    }

    private void OnContactTimeReached()
    {
        contactTime = 0f;
        isTubeInContact = false;
        vortexed = true;
        progress.fillAmount = 0f;
    }
}
