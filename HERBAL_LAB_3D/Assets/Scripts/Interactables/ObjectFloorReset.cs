using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloorReset : MonoBehaviour
{
    private Vector3 startingPosition;
    private Quaternion startingRotation; // To store the initial rotation
    private Rigidbody rb; // Reference to the Rigidbody component
    private float resetDelay = 0.5f;

    void Start()
    {
        // Store the initial position and rotation
        startingPosition = transform.position;
        startingRotation = transform.rotation;

        // Get the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody component missing from GameObject - required for physics reset.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object colliding with is tagged as "Floor"
        if (collision.gameObject.tag == "Floor")
        {
            StartCoroutine(ResetAfterDelay(resetDelay));
        }
    }

    IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null);
        {
            // Reset position, rotation, and velocities
            transform.position = startingPosition;
            transform.rotation = startingRotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
