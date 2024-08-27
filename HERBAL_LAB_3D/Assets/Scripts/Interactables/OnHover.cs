using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverHandler : MonoBehaviour
{
    public UnityEvent onHoverEnter; // Event to be invoked when the object is hovered
    public UnityEvent onHoverExit;  // Event to be invoked when the hover exits

    private bool isHovered = false; // Tracks if the object is currently being hovered over

    void Update()
    {
        // Perform a raycast from the camera to detect if hovering over the object, ignoring trigger colliders
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (!isHovered)
                {
                    OnHoverEnter();
                }
            }
            else
            {
                if (isHovered)
                {
                    OnHoverExit();
                }
            }
        }
        else
        {
            if (isHovered)
            {
                OnHoverExit();
            }
        }
    }

    void OnHoverEnter()
    {
        isHovered = true;
        onHoverEnter.Invoke(); // Invoke the hover enter event
    }

    void OnHoverExit()
    {
        isHovered = false;
        onHoverExit.Invoke(); // Invoke the hover exit event
    }
}
