using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapFromHand : MonoBehaviour
{
    public Transform lockPosition;
    public string tag;

    public float snapSpeed = 5f;
    private GameObject inputObject;
    public List<GameObject> showObjectAfterSnap;
    
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

            StartCoroutine(SmoothSnap(inputObject.transform));


            // Destroy Rb
            Rigidbody rb = inputObject.GetComponent<Rigidbody>();
            Destroy(rb);

            // Set object
            inputObject.transform.rotation = lockPosition.rotation;
            inputObject.transform.position = lockPosition.position;

            // Disable lockPosition collider
            Collider collider = lockPosition.GetComponent<Collider>();            
            collider.enabled = false;

            if (showObjectAfterSnap != null)
            {
                foreach (GameObject objectToShow in showObjectAfterSnap)
                {
                    objectToShow.SetActive(true);
                }
            }
        }
    }

    private IEnumerator SmoothSnap(Transform objectTransform)
    {
        while (Vector3.Distance(objectTransform.position, lockPosition.position) > 0.01f)
        {
            objectTransform.position = Vector3.Lerp(objectTransform.position, lockPosition.position, Time.deltaTime * snapSpeed);
            objectTransform.rotation = Quaternion.Slerp(objectTransform.rotation, lockPosition.rotation, Time.deltaTime * snapSpeed);
            yield return null;
        }

        objectTransform.position = lockPosition.position;
        objectTransform.rotation = lockPosition.rotation;

        // Optionally, parent the object to the lock position
        objectTransform.SetParent(lockPosition);
    }
}
