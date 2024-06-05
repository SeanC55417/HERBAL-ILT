using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapFromHand : MonoBehaviour
{
    public Transform lockPosition;
    public string tag;

    private GameObject inputObject;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(tag))
        { 
            // Check if PickupObject script exists on any game object
            PickupObject pickupScript = FindObjectOfType<PickupObject>();
            if (pickupScript != null)
            {   
                inputObject = pickupScript.GetObject();
                pickupScript.Drop(); // Call Drop() if PickupObject script is found
            }

            inputObject.transform.SetParent(lockPosition);

            // Destroy Rb
            Rigidbody rb = inputObject.GetComponent<Rigidbody>();
            Destroy(rb);

            // Set object
            inputObject.transform.rotation = lockPosition.rotation;
            inputObject.transform.position = lockPosition.position;

            // Disable lockPosition collider
            Collider collider = lockPosition.GetComponent<Collider>();            
            collider.enabled = false;

        }
    }
}
