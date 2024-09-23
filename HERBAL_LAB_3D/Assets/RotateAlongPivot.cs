using UnityEngine;

public class RotateAroundPivot : MonoBehaviour
{
    public Vector3 pivotPoint = Vector3.zero; // The pivot point for rotation
    public Vector3 rotationAxis = Vector3.up; // The axis to rotate around
    public float rotationDuration = 5f; // The duration of the rotation in seconds
    public float rotationAngle = 360f; // The angle to rotate (one full rotation)

    private float elapsedTime = 0f; // Time elapsed since the rotation started
    private bool isRotating = false; // Flag to check if rotation should start

    void Update()
    {
        if (isRotating)
        {
            if (elapsedTime < rotationDuration)
            {
                // Calculate the rotation step based on the duration
                float rotationStep = (rotationAngle / rotationDuration) * Time.deltaTime;
                // Rotate around the pivot point
                transform.RotateAround(pivotPoint, rotationAxis, rotationStep);
                elapsedTime += Time.deltaTime;
            }
            else
            {
                isRotating = false;
            }
        }
    }

    public void StartRotation()
    {
        if (!isRotating)
        {
            isRotating = true;
            elapsedTime = 0f;
        }
    }
}
