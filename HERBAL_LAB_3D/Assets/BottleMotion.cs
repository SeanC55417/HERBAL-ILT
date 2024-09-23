using UnityEngine;

public class BottleMotion : MonoBehaviour
{
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 velocity;
    private Vector3 angularVelocity;

    void Start()
    {
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    void Update()
    {
        // Calculate linear velocity
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;

        // Calculate angular velocity
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);
        angularVelocity = new Vector3(deltaRotation.eulerAngles.x, deltaRotation.eulerAngles.y, deltaRotation.eulerAngles.z) / Time.deltaTime;
        lastRotation = transform.rotation;

        // Pass the velocity and angular velocity to the shader
        Material material = GetComponent<Renderer>().material;
        material.SetVector("_ObjectVelocity", velocity);
        material.SetVector("_ObjectAngularVelocity", angularVelocity);
    }
}
