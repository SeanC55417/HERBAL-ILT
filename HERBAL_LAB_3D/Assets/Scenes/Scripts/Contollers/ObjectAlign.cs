using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectAlign : MonoBehaviour
{
    public enum Axis { x, y, z };
    public Axis alignAxis = Axis.x; // The axis along which the object should be aligned
    public float alignmentSpeed = 5f; // Speed at which the object returns to its aligned position
    public bool alignRotation = true; // Whether to align rotation as well
    public bool allowPositionChange = true; // Toggle for allowing position change
    public bool allowRotationChange = true; // Toggle for allowing rotation change

    public Vector3 targetRotation; // The target rotation values you want to set

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Store the initial position and rotation
        initialPosition = transform.position;
        initialRotation = Quaternion.Euler(targetRotation);

        // Ensure the object is initially aligned
        AlignObject();
    }

    void Update()
    {
        // If the object is moving, stop applying alignment until it slows down
        if (rb != null && rb.velocity.magnitude > 0.1f)
            return;

        // Align position if allowed
        if (allowPositionChange)
        {
            transform.position = Vector3.Lerp(transform.position, initialPosition, alignmentSpeed * Time.deltaTime);
        }

        // Align rotation if allowed and enabled
        if (alignRotation && allowRotationChange)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, alignmentSpeed * Time.deltaTime);
        }
    }

    void AlignObject()
    {
        Vector3 alignedPosition = initialPosition;

        // Align position based on the specified axis if allowed
        if (allowPositionChange)
        {
            switch (alignAxis)
            {
                case Axis.x:
                    alignedPosition.y = transform.position.y;
                    alignedPosition.z = transform.position.z;
                    break;
                case Axis.y:
                    alignedPosition.x = transform.position.x;
                    alignedPosition.z = transform.position.z;
                    break;
                case Axis.z:
                    alignedPosition.x = transform.position.x;
                    alignedPosition.y = transform.position.y;
                    break;
            }

            transform.position = alignedPosition;
        }

        // If aligning rotation is allowed and enabled, set the rotation to target values
        if (alignRotation && allowRotationChange)
        {
            transform.rotation = initialRotation;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When the object collides, you might want to apply some logic here.
        // For instance, if the object gets bumped, you could briefly disable alignment
        // and then re-enable it after a short delay.
        // StartCoroutine(RealignAfterBump());
        AlignObject();
    }

    IEnumerator RealignAfterBump()
    {
        // Disable alignment briefly after a bump
        yield return new WaitForSeconds(0.5f);
        AlignObject();
    }
}
