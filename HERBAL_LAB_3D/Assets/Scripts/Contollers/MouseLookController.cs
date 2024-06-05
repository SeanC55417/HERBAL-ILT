using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseLookController : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform player;
    public float targetFrameRate = 60f;

    private float rotationX = 0f;
    private float customDeltaTime;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; // Ensure the cursor is hidden
        customDeltaTime = 1.0f / targetFrameRate;
    }

    void Update()
    {
        HandleMouseMovement();
        PerformRaycast();
    }

    void HandleMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * customDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * customDeltaTime;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    void PerformRaycast()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            HandleInteraction(hit);
        }
    }

    void HandleInteraction(RaycastHit hit)
    {
        // Ensure the object has a Button component and is interactable
        Button button = hit.collider.gameObject.GetComponent<Button>();
        if (button != null && button.interactable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                button.onClick.Invoke();
            }
        }
    }
}
