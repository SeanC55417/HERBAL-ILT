using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDisableScript : MonoBehaviour
{
    void Start()
    {
        DisableAllXRComponents();
    }

    void DisableAllXRComponents()
    {
        // Disable all XR Controllers
        XRController[] controllers = FindObjectsOfType<XRController>();
        foreach (var controller in controllers)
        {
            controller.enabled = false;
        }

        // Disable all XR Interactors
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor[] interactors = FindObjectsOfType<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor>();
        foreach (var interactor in interactors)
        {
            interactor.enabled = false;
        }

        // Disable any additional XR components you may have
        XRBaseController[] baseControllers = FindObjectsOfType<XRBaseController>();
        foreach (var baseController in baseControllers)
        {
            baseController.enabled = false;
        }
        
        Debug.Log("All XR components disabled.");
    }
}
