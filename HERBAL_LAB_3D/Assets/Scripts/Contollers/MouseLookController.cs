using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseLookController : MonoBehaviour
{
    public float sensitivity = 100f;
    public Transform player;
    public float targetFrameRate = 60f;

    private float rotationX = 0f;
    private float customDeltaTime;
    public PickupObject pickupObjectScript;

    // Goes in labelScript
    public GameObject labelObject;
    private GameObject currentLabel;
    private HashSet<string> itemsWithDescriptionsSet;
    private float highestYPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        customDeltaTime = 1.0f / targetFrameRate;
        getItemDescriptions();
    }

    void Update()
    {
        HandleMouseMovement();
        PerformRaycast();

        if (currentLabel != null)
        {
            currentLabel.transform.LookAt(Camera.main.transform);
        }
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

        int layerMask = LayerMask.GetMask("Default", "UI", "Menu");

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
        {
            HandleInteraction(hit);
        }
    }

    void getItemDescriptions()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        TextAsset csvFile = Resources.Load<TextAsset>("Data/" + currentScene.name + " Item Descriptions");
        // Debug.Log("Data/" + currentScene.name + "Lab Item Descriptions");
        // TextAsset csvFile = Resources.Load<TextAsset>("Data/HPLC Lab Item Descriptions");
        using (StringReader sr = new StringReader(csvFile.text))
        {
            itemsWithDescriptionsSet = new HashSet<string>();
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                string[] columns = line.Split(',');

                if (columns.Length > 0)
                {
                    string firstColumnValue = columns[0];
                    itemsWithDescriptionsSet.Add(firstColumnValue);
                }
            }
        }
    }

    private IEnumerator Wait(float secondsWaiting)
    {
        yield return new WaitForSeconds(secondsWaiting);
    }

    void showLabel(GameObject itemSearch)
    {
        if (currentLabel == null && itemsWithDescriptionsSet.Contains(itemSearch.name) && pickupObjectScript.heldObject == null)
        {
            currentLabel = Instantiate(labelObject, player.transform.parent.transform);
            // currentLabel.transform.localScale = Vector3.one; // Fix the scale
            
            // currentLabel.transform.position = itemSearch.transform.position;
            highestYPosition = 0;
            FindHighestObject(itemSearch);
            // Debug.Log("highest Y position: " + highestYPosition);
            currentLabel.transform.position = new Vector3(itemSearch.transform.position.x, highestYPosition + .35f, itemSearch.transform.position.z);
            // currentLabel.transform.SetParent(itemSearch.transform);
            TextMeshProUGUI labelText = currentLabel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (labelText != null)
            {
                labelText.text = itemSearch.name;
                // Debug.Log("Show item label for " + itemSearch.name);
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component not found on label object.");
            }
        }
        else if (pickupObjectScript.heldObject != null && currentLabel != null)
        {
            Destroy(currentLabel);
        }
    }

    void HandleInteraction(RaycastHit hit)
    {
        GameObject hitObject = hit.collider.gameObject;
        Button button = hitObject.GetComponent<Button>();
        if (button != null && button.interactable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                button.onClick.Invoke();
            }
        }

        Debug.Log(hitObject.name);
        if (itemsWithDescriptionsSet != null && itemsWithDescriptionsSet.Contains(hitObject.name))
        {
            if (itemsWithDescriptionsSet.Contains(hitObject.name))
            {
                showLabel(hitObject);
            }
        }
        else if (itemsWithDescriptionsSet != null && itemsWithDescriptionsSet.Contains(hitObject.transform.parent.name))
        {
            if (itemsWithDescriptionsSet.Contains(hitObject.name))
            {
                showLabel(hitObject.transform.parent.gameObject);
            }
        }
        else if (currentLabel != null)
        {
            Destroy(currentLabel);
            currentLabel = null;
        }
    }

    float FindHighestObject(GameObject Item)
    {
        // Check the condition for this object
        if (Item.transform.position.y > highestYPosition)
        {
            highestYPosition = Item.transform.position.y;
            
        }

        // Iterate through all children of the current object
        foreach (Transform child in Item.transform)
        {
            FindHighestObject(child.gameObject);
        }

        return highestYPosition;
    }

    // USE THIS TO FIND THE TOP OF OBJECTS ITERATING THROUGH CHILD OBJECTS
    // GameObject FindHighestObject(GameObject Item)
    // {
    //     GameObject highestObject = null;
    //     float highestValue = float.MinValue;

    //     
    //         if (item != null)
    //         {
    //             // Get object's y position
    //             float yPosition = obj.transform.position.y;
                
    //             // Get object's height if it has a collider
    //             float objectHeight = 0f;
    //             Collider objCollider = obj.GetComponent<Collider>();
    //             if (objCollider != null)
    //             {
    //                 objectHeight = objCollider.bounds.size.y;
    //             }

    //             // Combine y position and height
    //             float totalHeight = yPosition + objectHeight;

    //             // Check if it's the highest so far
    //             if (totalHeight > highestValue)
    //             {
    //                 highestValue = totalHeight;
    //                 highestObject = obj;
    //             }
    //         }
    //     }

    //     return highestObject;
    // }
}

// IEnumerator HideLabelWithDelay()
// {
//     yield return new WaitForSeconds(delayTime); // Wait for a short delay before hiding the label
//     if (currentLabel != null)
//     {
//         Destroy(currentLabel);
//         currentLabel = null;
//     }
// }

/*

private Coroutine labelCoroutine;
private float hoverDelay = 0.5f; // Half a second delay

void showLabel(GameObject itemSearch)
{
    if (currentLabel == null && itemsWithDescriptionsSet.Contains(itemSearch.name) && pickupObjectScript.heldObject == null)
    {
        if (labelCoroutine == null) // Start the coroutine only if it's not already running
        {
            labelCoroutine = StartCoroutine(ShowLabelAfterDelay(itemSearch));
        }
    }
    else if (pickupObjectScript.heldObject != null && currentLabel != null)
    {
        Destroy(currentLabel);
        currentLabel = null;
        StopLabelCoroutine();
    }
}

IEnumerator ShowLabelAfterDelay(GameObject itemSearch)
{
    yield return new WaitForSeconds(hoverDelay); // Wait for the hover delay before showing the label

    if (currentLabel == null) // Ensure the label hasn't already been shown
    {
        currentLabel = Instantiate(labelObject, player.transform.parent.transform);
        currentLabel.transform.position = new Vector3(itemSearch.transform.position.x, itemSearch.transform.position.y + 0.5f, itemSearch.transform.position.z);
        TextMeshProUGUI labelText = currentLabel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        if (labelText != null)
        {
            labelText.text = itemSearch.name;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not found on label object.");
        }
    }
}

void HandleInteraction(RaycastHit hit)
{
    GameObject hitObject = hit.collider.gameObject;

    
}

void StopLabelCoroutine()
{
    if (labelCoroutine != null)
    {
        StopCoroutine(labelCoroutine); // Stop the coroutine if it's running
        labelCoroutine = null;
    }
}


*/