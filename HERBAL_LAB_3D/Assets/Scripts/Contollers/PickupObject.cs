using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour
{
    public Transform player;
    public Transform playerCamera;
    public float pickupRange = 5f;
    public float holdDistance = 2f;
    private GameObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, pickupRange))
                {
                    Pickup(hit.transform.gameObject);
                }
            }
            else
            {
                Drop();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldObject != null)
            {
                RotationReset(heldObject);
            }
        }
    }

    void Pickup(GameObject pickObject)
    {
        if (pickObject.GetComponent<Rigidbody>())
        {
            Rigidbody objectRb = pickObject.GetComponent<Rigidbody>();
            objectRb.useGravity = false;
            objectRb.drag = 10;

            objectRb.transform.parent = playerCamera;
            objectRb.transform.localPosition = new Vector3(0f, 0f, holdDistance);
            heldObject = pickObject;
        }
    }

    void Drop()
    {
        if (heldObject.GetComponent<Rigidbody>())
        {
            Rigidbody heldRb = heldObject.GetComponent<Rigidbody>();
            heldRb.useGravity = true;
            heldRb.drag = 1;

            heldObject.transform.parent = null;
            heldObject = null;
        }
    }

    void RotationReset(GameObject pickObject)
    {
        Rigidbody objectRb = pickObject.GetComponent<Rigidbody>();
        if (objectRb != null)
        {
            objectRb.angularVelocity = Vector3.zero; // Stop any ongoing rotation
            objectRb.transform.localRotation = Quaternion.Euler(0f, 90f, 0f); // Reset the rotation
            objectRb.transform.localPosition = new Vector3(0f, 0f, holdDistance); // Reset position in front of player
        }
    }
}