using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class DebugPlayerController : MonoBehaviour
{
    // Speed Modes
    public enum SpeedMode { Normal, Fast, Super }
    public SpeedMode currentSpeedMode = SpeedMode.Normal;

    // Controller Variables
    private CharacterController characterController;
    public Camera playerCamera;
    public float baseSpeed = 10f;
    public float jumpPower = 350f;
    public float gravity = 900f;

    // Speed multipliers for each mode
    private readonly Dictionary<SpeedMode, float> speedMultipliers = new()
    {
        { SpeedMode.Normal, 1f },
        { SpeedMode.Fast, 2f },
        { SpeedMode.Super, 8f }
    };

    // Mouse look
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    // Perspective Scroll
    public float scrollSpeed = 5f;
    public float minDistance = -2f;
    public float maxDistance = 10f;
    public float currentDistance = 0f;
    private readonly float defaultDistance = 0f;
    private readonly float defaultCamHeightOffset = 1.3f;

    // Player Movement
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    // Movement States
    public bool canMove = true;
    public bool isFlying = false;

    // UI Display
    public TextMeshProUGUI hud_text_mode;
    public TextMeshProUGUI hud_text_speed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateHUD();
    }

    void Update()
    {
        HandleModeSwitch();
        HandleSpeedSwitch();
        HandleMovement();
        HandleMouseLook();
        ScrollPerspective();
    }

    void HandleModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isFlying = !isFlying;
            hud_text_mode.text = isFlying ? "Flying" : "Walking";

            if (!isFlying)
            {
                // Apply gravity immediately when switching back to walking mode
                moveDirection.y = -gravity * Time.deltaTime;
            }
        }
    }

    void HandleSpeedSwitch()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Cycle through speed modes
            currentSpeedMode = (SpeedMode)(((int)currentSpeedMode + 1) % System.Enum.GetValues(typeof(SpeedMode)).Length);
            UpdateHUD();
        }
    }

    void HandleMovement()
    {
        if (canMove)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            float speedMultiplier = speedMultipliers[currentSpeedMode];
            float curSpeedX = baseSpeed * speedMultiplier * Input.GetAxis("Vertical");
            float curSpeedY = baseSpeed * speedMultiplier * Input.GetAxis("Horizontal");

            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (isFlying)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    moveDirection.y = baseSpeed * speedMultiplier; // Move up
                }
                else if (Input.GetKey(KeyCode.Q))
                {
                    moveDirection.y = -baseSpeed * speedMultiplier; // Move down
                }
                else
                {
                    moveDirection.y = 0; // Maintain current altitude
                }
            }
            else
            {
                if (characterController.isGrounded)
                {
                    if (Input.GetButton("Jump"))
                    {
                        moveDirection.y = jumpPower;
                    }
                    else
                    {
                        moveDirection.y = -gravity * Time.deltaTime; // Apply slight gravity to keep grounded
                    }
                }
                else
                {
                    moveDirection.y -= gravity * Time.deltaTime; // Apply gravity while in the air
                }
            }

            characterController.Move(moveDirection * Time.deltaTime);
        }
    }

    void HandleMouseLook()
    {
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            float rotationY = Input.GetAxis("Mouse X") * lookSpeed;
            transform.Rotate(0, rotationY, 0);
        }
    }

    void ScrollPerspective()
    {
        // Scroll the mouse wheel to adjust the camera distance
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentDistance -= scroll * scrollSpeed;
        currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);

        // Check for middle mouse button click to reset perspective
        if (Input.GetMouseButtonDown(2))
        {
            ResetCameraPosition();
        }
        else
        {
            playerCamera.transform.GetLocalPositionAndRotation(out Vector3 newPosition, out Quaternion newRotation);
            newPosition.z = -currentDistance;

            // Optionally, you can adjust the height for a slightly elevated view
            newPosition.y = Mathf.Lerp(1f, 4f, (currentDistance - minDistance) / (maxDistance - minDistance));

            playerCamera.transform.SetLocalPositionAndRotation(newPosition, newRotation);
        }
    }

    void ResetCameraPosition()
    {
        currentDistance = defaultDistance;
        Vector3 resetPosition = new Vector3(0, defaultCamHeightOffset, -currentDistance);
        playerCamera.transform.localPosition = resetPosition;
    }

    void UpdateHUD()
    {
        hud_text_speed.text = currentSpeedMode.ToString();
    }
}
