using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Microscope : MonoBehaviour
{
    
    public GameObject sampleGameObject;
    public Transform loadingTarget;
    public bool microscopeActive = true;
    private PickupObject pickupObject;
    private bool viewing;
    private bool isSampleLoaded;

    void Start()
    {
        pickupObject = FindObjectOfType<PickupObject>();
    }
    void Update()
    {
        if (viewing)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            {
                viewing = false;
                ViewToggle();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == sampleGameObject && microscopeActive)
        {
            pickupObject.Take();
            sampleGameObject.transform.SetParent(null);
            sampleGameObject.transform.SetLocalPositionAndRotation(loadingTarget.position, loadingTarget.rotation);
            isSampleLoaded = true;
            Debug.Log("ENTERED");
        }
    }

    private void ViewToggle()
    {

    }

    public void OnClicked()
    {
        Debug.Log("CLICKED INTO MICROSCOPE");
        viewing = true;
        // insert magic here
        ViewToggle();
    }

    public bool IsSampleLoaded()
    {
        return isSampleLoaded;
    }

    public bool IsViewing()
    {
        return viewing;
    }
}
