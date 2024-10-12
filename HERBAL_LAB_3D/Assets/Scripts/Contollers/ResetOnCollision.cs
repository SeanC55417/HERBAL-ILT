using UnityEngine;

public class ResetOnCollision : MonoBehaviour
{
    // The layer that represents the floor
    public LayerMask floorLayer;

    // The local position to reset the object to
    private Vector3 initialLocalPosition;

    // The local rotation to reset the object to
    private Quaternion initialLocalRotation;

    // The original parent of the object
    private Transform originalParent;

    void Start()
    {
        // Store the initial local position and rotation of the object
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        originalParent = transform.parent;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is on the floor layer
        if ((floorLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            ResetPositionAndRotation();
        }
    }

    void ResetPositionAndRotation()
    {
        // Reassign the object to its original parent
        transform.SetParent(originalParent, true);

        // Reset the object's local position and rotation
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;

        // Optionally reset the object's velocity if it has a Rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
