using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform player;
    public Transform playerCamera;
    public Transform handPosition; // The transform representing the player's hand position
    public float pickupRange = 5f;
    public float defaultHoldDistance = 1f;
    private float currentHoldDistance;
    private GameObject heldObject;
    private bool isEquipped = false;
    public float scrollSensitivity = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                // Raycast with QueryTriggerInteraction.Ignore to ignore trigger colliders
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickupRange, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
                {
                    Pickup(hit.transform.gameObject, hit.distance);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (heldObject != null && !isEquipped)
            {
                Drop();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject != null)
            {
                if (isEquipped)
                {
                    Drop();
                }
                else
                {
                    Equip();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && heldObject != null)
        {
            RotationReset(heldObject);
        }

        if (heldObject != null && !isEquipped)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0f)
            {
                AdjustHoldDistance(scrollInput);
            }
        }
    }

    void Pickup(GameObject pickObject, float distance)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            Rigidbody objectRb = pickObject.GetComponent<Rigidbody>();
            objectRb.useGravity = false;
            objectRb.drag = 10;

            currentHoldDistance = distance < defaultHoldDistance ? distance : defaultHoldDistance;

            objectRb.transform.parent = playerCamera;
            objectRb.transform.localPosition = new Vector3(0f, 0f, currentHoldDistance);
            heldObject = pickObject;
        }
    }

    public void Drop()
    {
        if (heldObject && heldObject.GetComponent<Rigidbody>())
        {
            Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
            heldRb.useGravity = true;
            heldRb.drag = 1;

            Transform objectTransform = heldObject.transform;
            heldObject.transform.parent = null;

            // Raycast to check if there's an object within range where the object can be dropped
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickupRange, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                // Set position above the hit point (e.g., a table) and reset rotation
                Vector3 dropPosition = hit.point + Vector3.up * 0.1f; // Slightly above the hit point to avoid clipping
                heldRb.transform.SetPositionAndRotation(dropPosition, Quaternion.identity);
            }
            else
            {
                // No object in range, drop at default distance and reset rotation
                Vector3 dropPosition = playerCamera.position + playerCamera.forward * currentHoldDistance;
                heldRb.transform.SetPositionAndRotation(dropPosition, Quaternion.identity);
            }

            heldObject = null;
            isEquipped = false;
        }
    }

    void Equip()
    {
        if (heldObject != null)
        {
            Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
            heldRb.useGravity = false;
            heldRb.drag = 10;

            // Get the object's preferred position and rotation
            PickableObjectProperties properties = heldObject.GetComponent<PickableObjectProperties>();
            Vector3 preferredPosition = properties != null ? properties.preferredHeldPosition : Vector3.zero;
            Quaternion preferredRotation = properties != null ? properties.preferredHeldRotation : Quaternion.identity;

            // Set the object's position and rotation relative to the hand position
            heldObject.transform.parent = handPosition;
            heldObject.transform.localPosition = preferredPosition;
            heldObject.transform.localRotation = preferredRotation;
            isEquipped = true;
        }
    }

    public GameObject GetObject()
    {
        return heldObject;
    }

void RotationReset(GameObject pickObject)
{
    Rigidbody objectRb = pickObject.GetComponent<Rigidbody>();
    if (objectRb != null)
    {
        objectRb.angularVelocity = Vector3.zero;

        if (isEquipped)
        {
            // Reset rotation and position relative to the hand when equipped
            PickableObjectProperties properties = pickObject.GetComponent<PickableObjectProperties>();

            // If the object has custom properties, use them; otherwise, reset to defaults
            if (properties != null)
            {
                pickObject.transform.localPosition = properties.preferredHeldPosition;
                pickObject.transform.localRotation = properties.preferredHeldRotation;
            }
            else
            {
                pickObject.transform.localPosition = Vector3.zero;
                pickObject.transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            // Reset rotation and position in front of the camera when not equipped
            objectRb.transform.localPosition = new Vector3(0f, 0f, currentHoldDistance);
            objectRb.transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
        }
    }
}


    void AdjustHoldDistance(float scrollInput)
    {
        currentHoldDistance += scrollInput * scrollSensitivity;
        currentHoldDistance = Mathf.Clamp(currentHoldDistance, 0.5f, pickupRange);

        if (heldObject != null)
        {
            heldObject.transform.localPosition = new Vector3(0f, 0f, currentHoldDistance);
        }
    }

    public void take()
    {
        heldObject = null;
        isEquipped = false;
    }
}
