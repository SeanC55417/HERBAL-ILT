using System.Collections;
using UnityEngine;

public class Centrifuge : MonoBehaviour
{
    public GameObject tube; // Reference to the tube GameObject
    public GameObject mixed;
    public GameObject separated;
    public Transform hinge; // Reference to the hinge Transform
    public Transform tubePlacement; // Reference to the position where the tube should be placed
    public PickupObject pickupObject;

    private bool isAnimating = false; // To track if the animation is playing
    private readonly float rotationSpeed = 90f; // degrees per second
    private bool centrifugeActive = true; //! here later false
    private bool centrifuged = false;

    public bool CentrifugeActive
    {
        get { return centrifugeActive; }
        set { centrifugeActive = value; }
    }

    public bool Centrafuged
    {
        get { return centrifuged; }
        set { centrifuged = value; }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the tube
        if (centrifugeActive && other.gameObject == tube && !isAnimating)
        {
            // Move the tube to the predefined placement position
            PlaceTube();

            // Start the hinge rotation sequence
            StartCoroutine(HingeRotationSequence());

        }
    }

    private void PlaceTube()
    {
        if (tubePlacement != null && tube != null)
        {
            pickupObject.Take();

            // Ensure the tube is not parented to any other object to prevent unexpected movement
            tube.transform.SetParent(null);

            // Set the tube's position and rotation to match the tubePlacement
            tube.transform.SetPositionAndRotation(tubePlacement.position, tubePlacement.rotation);

            // // Disable the Rigidbody to prevent physics from affecting the tube
            // Rigidbody tubeRigidbody = tube.GetComponent<Rigidbody>();
            // if (tubeRigidbody != null)
            // {
            //     tubeRigidbody.isKinematic = true;
            //     tubeRigidbody.detectCollisions = false;
            // }

            Debug.Log("Tube placed at the centrifuge position, Rigidbody disabled.");
        }
        else
        {
            if (tubePlacement == null)
                Debug.LogError("Tube placement position not assigned!");

            if (tube == null)
                Debug.LogError("Tube object not assigned!");
        }
    }

    private IEnumerator HingeRotationSequence()
    {
        isAnimating = true;

        // Close the hinge (rotate 90 degrees around the X-axis)
        yield return RotateHinge(new Vector3(90, 0, 0));

        // Wait for 4 seconds while the hinge remains closed
        yield return new WaitForSeconds(4f);

        LayerTubeLiquid();

        // Open the hinge back to the original position
        yield return RotateHinge(Vector3.zero);

        isAnimating = false;
        centrifuged = true;
    }

    private IEnumerator RotateHinge(Vector3 targetEulerAngles)
    {
        Quaternion initialRotation = hinge.localRotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        float elapsedTime = 0f;

        // Rotate the hinge over 1 second
        while (elapsedTime < 1f)
        {
            hinge.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is set correctly
        hinge.localRotation = targetRotation;
    }

    private void LayerTubeLiquid()
    {
        mixed.SetActive(false);
        separated.SetActive(true);
    }
}
