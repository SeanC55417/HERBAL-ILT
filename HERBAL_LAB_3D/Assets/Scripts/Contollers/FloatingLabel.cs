using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingLabel : MonoBehaviour
{
    public Transform player;
    public Collider collider; // For trigger deactivation

    public float bobSpeed = 1f; // Speed of the bobbing motion
    public float bobAmount = 0.1f; // Amount of movement in the bobbing motion
    
    private float startingY;
    private float currentY; // Stores the current y position for bobbing
    private bool trackCollider = false;

    void Start()
    {
        if (collider != null){
            trackCollider = true;
        }
        startingY = transform.localPosition.y;
        currentY = startingY;
    }

    void Update()
    {
        // Look at player
        if (player != null){
            transform.LookAt(player.transform.position);
            transform.Rotate(0, 180, 0);
        }
        if (trackCollider){
            if (collider.enabled == false){
                gameObject.SetActive(false);
            }
        }
        // Bobbing motion
        currentY = (Mathf.Sin(Time.time * bobSpeed) * bobAmount + currentY)/4;
        transform.localPosition = new Vector3(transform.localPosition.x, currentY + startingY, transform.localPosition.z);
    }
}
