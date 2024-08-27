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

    private int originalLayer; // Store the original layer of the held object

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

            // Store the original layer of the held object
            originalLayer = heldObject.layer;
        }
    }

    public void Drop()
    {
        if (heldObject && heldObject.GetComponent<Rigidbody>())
        {
            Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
            heldRb.useGravity = true;
            heldRb.drag = 1;

            // Re-enable the collider
            Collider heldCollider = heldObject.GetComponent<Collider>();
            if (heldCollider != null)
            {
                heldCollider.enabled = true;
            }

            // Reset the object's layer and its children's layers to the original
            SetLayerRecursively(heldObject, originalLayer);

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
            heldObject.transform.SetLocalPositionAndRotation(preferredPosition, preferredRotation);
            isEquipped = true;

            // Disable the collider
            Collider heldCollider = heldObject.GetComponent<Collider>();
            if (heldCollider != null)
            {
                heldCollider.enabled = false;
            }

            // Change the object's layer and its children's layers to "Menu"
            SetLayerRecursively(heldObject, LayerMask.NameToLayer("Menu"));
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
                    pickObject.transform.SetLocalPositionAndRotation(properties.preferredHeldPosition, properties.preferredHeldRotation);
                }
                else
                {
                    pickObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                }
            }
            else
            {
                // Reset rotation and position in front of the camera when not equipped
                objectRb.transform.SetLocalPositionAndRotation(new Vector3(0f, 0f, currentHoldDistance), Quaternion.Euler(0f, 90f, 0f));
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

    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    // Helper method to set the layer of an object and its children recursively
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
