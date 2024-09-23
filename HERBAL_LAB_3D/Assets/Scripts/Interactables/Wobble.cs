using UnityEngine;

public class Wobble : MonoBehaviour
{
    private Renderer rend;
    private Vector3 lastPos;
    private Vector3 velocity;
    private Vector3 lastRot;
    private Vector3 angularVelocity;
    
    public float MaxWobble = 0.1f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    public float VelocityMultiplier = 200f; // New multiplier for increasing velocity impact
    
    private float wobbleAmountX;
    private float wobbleAmountZ;
    private float wobbleAmountToAddX;
    private float wobbleAmountToAddZ;
    private float pulse;
    private float time = 0.5f;
    
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;
    }
    
    private void Update()
    {
        time += Time.deltaTime;

        // Decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * Recovery);
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * Recovery);

        // Make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // Send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);

        // Calculate velocity
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;

        // Calculate angular velocity
        Vector3 deltaRotation = transform.rotation.eulerAngles - lastRot;
        // Normalize angles to be within -180 to 180 degrees
        deltaRotation.x = Mathf.DeltaAngle(lastRot.x, transform.rotation.eulerAngles.x);
        deltaRotation.y = Mathf.DeltaAngle(lastRot.y, transform.rotation.eulerAngles.y);
        deltaRotation.z = Mathf.DeltaAngle(lastRot.z, transform.rotation.eulerAngles.z);
        angularVelocity = deltaRotation / Time.deltaTime;
        lastRot = transform.rotation.eulerAngles;

        // Add clamped velocity to wobble, with increased impact
        wobbleAmountToAddX += Mathf.Clamp((velocity.x * VelocityMultiplier + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z * VelocityMultiplier + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
    }
}
