using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine;

// MouseLook rotates the player object and camera based on mouse input.
public class MouseLookController : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform player;

    private float rotationX = 0f;

    // Initialize settings
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseMovement();
        PerformRaycast();
    }

    // Handle mouse movements for looking around
    void HandleMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Apply rotation to camera (X-axis) and player (Y-axis)
        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    // Perform a raycast to interact with UI elements
    void PerformRaycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            HandleInteraction(hit);
        }
    }

    // Handle interactions with objects hit by raycast
    void HandleInteraction(RaycastHit hit)
    {
        // Simulate button clicks if a button is hit
        if (hit.collider.gameObject.CompareTag("Button"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                hit.collider.gameObject.GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}
