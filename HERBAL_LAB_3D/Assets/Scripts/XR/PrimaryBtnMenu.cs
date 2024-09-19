using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Readers;
using UnityEngine.InputSystem;

public class PrimaryBtnMenu : MonoBehaviour
{
    // This will allow you to drag and drop the InputAction in the Inspector
    public InputActionProperty primaryButtonAction;
    private FloatingNotebookScript floatingNotebook;

    // Start method if needed to enable Input Action
    private void Start()
    {
        primaryButtonAction.action.Enable();
        floatingNotebook = FindAnyObjectByType<FloatingNotebookScript>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the primary button is pressed
        if (primaryButtonAction.action.WasPressedThisFrame())
        { 
            floatingNotebook.setNotebook();
        }
    }

    private void OnDestroy()
    {
        primaryButtonAction.action.Disable();
    }
}
