using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using TMPro;

public class MouseLookController : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform player;
    public float targetFrameRate = 60f;

    private float rotationX = 0f;
    private float customDeltaTime;

    float timeRequiredForLabel = 2.0f;
    float hitTime = 0.0f;
    bool isHitting = false;

    public GameObject labelObject;
    private Dictionary<string, int> itemIndexes = new Dictionary<string, int>();
    private bool showingLabel = false;
    GameObject currentLabel = null;
    public PickupObject pickupObjectScript;

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
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            HandleInteraction(hit);
        }
    }

    void showLabel(GameObject item)
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/HPLC Lab Item Descriptions");
        using (StringReader sr = new StringReader(csvFile.text))
        {
            List<string> firstColumnValues = new List<string>();
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');

                if (columns.Length > 0)
                {
                    string firstColumnValue = columns[0];
                    firstColumnValues.Add(firstColumnValue);
                }
            }

            // Process the first column values
            foreach (string value in firstColumnValues)
            {
                if (item.name == value)
                {
                    showingLabel = true;
                    Vector3 itemPosition = item.transform.position;
                    float newYPosition = item.transform.position.y + 0.4f;

                    
                    
                    Vector3 targetPosition = new Vector3(itemPosition.x, newYPosition, itemPosition.z);

                    currentLabel = Instantiate(labelObject);
                    currentLabel.transform.position = targetPosition;
                    currentLabel.transform.LookAt(player.transform.position);
                    currentLabel.transform.Rotate(0, 180, 0);

                    TextMeshProUGUI currentLabelText = currentLabel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    currentLabelText.text = value;
                }
            }
        }

    }

    void HandleInteraction(RaycastHit hit)
    {
        // Ensure the object has a Button component and is interactable
        GameObject hitObject = hit.collider.gameObject;
        Button button = hitObject.GetComponent<Button>();
        if (button != null && button.interactable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                button.onClick.Invoke();
            }
        }
        else if (hitObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            if (showingLabel == false && pickupObjectScript.GetObject() == null)
            {
                showLabel(hitObject);
            }
        }
        else
        {
            showingLabel = false;
            Destroy(currentLabel);
        }




        // else if (hit.collider.gameObject.CompareTag("SampleBottle"))
        // {
        //     if (!isHitting)
        //     {
        //         isHitting = true;
        //         hitTime = Time.time;
        //     }
        //     else
        //     {
        //         if (Time.time - hitTime >= timeRequiredForLabel)
        //         {
        //             Debug.Log("label");
                    
        //             Transform labelTransform = labelObject.transform;
        //             // Vector3 targetPosition = labelTransform.anchoredPosition

        //             // Makes the label look at the player
        //             labelTransform.LookAt(player.position);
        //             labelTransform.Rotate(0, 180, 0);

        //             // Optionally, make the label object active
        //             labelObject.SetActive(true);
        //         }
        //     }
        // }
        // else
        // {
        //     isHitting = false;
        //     hitTime = 0.0f; // Reset the hit time
        //     labelObject.SetActive(false); // Hide the label if not hitting the target
        // }
    }
}
