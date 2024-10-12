using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerSlide : MonoBehaviour
{
    public Transform drawer = null; // Reference to the drawer object

    private bool isOpen = false;
    private bool isAnimating = false; // To track if the animation is playing
    public float slideSpeed = .5f; // Units per second
    public Vector3 closedPosition; // Local position of the drawer when closed
    public Vector3 openPosition; // Local position of the drawer when open

    public void ToggleDrawer()
    {
        if (!isAnimating) // Only allow toggling if not currently animating
        {
            StartCoroutine(AnimateDrawer());
        }
    }

    private IEnumerator AnimateDrawer()
    {
        isAnimating = true;

        Vector3 targetPosition = isOpen ? closedPosition : openPosition;

        while (drawer.localPosition != targetPosition)
        {
            drawer.localPosition = Vector3.MoveTowards(drawer.localPosition, targetPosition, slideSpeed * Time.deltaTime);
            yield return null;
        }

        isOpen = !isOpen;
        isAnimating = false;
    }
}
