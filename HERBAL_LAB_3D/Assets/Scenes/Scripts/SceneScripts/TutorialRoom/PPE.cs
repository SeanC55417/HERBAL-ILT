using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PPE : MonoBehaviour
{
    public TutorialManager tutorialManager;

    public TextMeshProUGUI playerHudText; 
    public ParticleSystem confetti;
    public Transform ppe;                           // Object containing ppe
    public GameObject[] initialPPEObjects;          // Initial state of PPE objects
    public int confettiTime = 3;                    // Seconds

    public float rotationSpeed = 360.0f; // Degrees per second
    public int numberOfSpins = 3;
    public float duration = 1.0f; // Duration to complete all spins

    private bool hasChecked = false; // Flag to ensure the check runs only once

    void Update()
    {
        if (ppe.childCount == 0 && !hasChecked)
        {
            hasChecked = true;
            this.enabled = false;
        }

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Transform hitTransform = hit.transform;
                if (hitTransform.parent == ppe)
                {
                    StartCoroutine(SpinOut(hitTransform));
                }
            }
        }
    }

    IEnumerator SpinOut(Transform target)
    {
        float totalRotation = 360 * numberOfSpins;
        float currentRotation = 0;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float rotationStep = rotationSpeed * Time.deltaTime;
            target.Rotate(0, rotationStep, 0); // Adjust axis if needed
            currentRotation += rotationStep;
            timeElapsed += Time.deltaTime;

            if (currentRotation >= totalRotation)
                break;

            yield return null;
        }

        Destroy(target.gameObject); // Destroys the GameObject after spinning
    }

    void StartConfetti()
    {
        if (confetti != null && playerHudText != null)
        {   
            confetti.Play();
            StartCoroutine(StopConfettiAfterDelay(confettiTime));
        }
    }

    IEnumerator StopConfettiAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        confetti.Stop();
    }

    public void ResetPPECheck()
    {
        hasChecked = false;
        playerHudText.text = "";

        foreach (Transform child in ppe)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject obj in initialPPEObjects)
        {
            Instantiate(obj, ppe);
        }

        this.enabled = true;
    }
}
