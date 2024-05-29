using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform leftHinge = null;
    public Transform rightHinge = null;

    private bool isOpen = false;
    private bool isAnimating = false; // To track if the animation is playing
    private float rotationSpeed = 90f; // degrees per second

    public void ToggleDoor()
    {
        if (!isAnimating) // Only allow toggling if not currently animating
        {
            StartCoroutine(AnimateDoors());
        }
    }

    private IEnumerator AnimateDoors()
    {
        isAnimating = true;

        Quaternion leftTargetRotation = leftHinge ? Quaternion.Euler(0, isOpen ? 0 : 90, 0) : Quaternion.identity;
        Quaternion rightTargetRotation = rightHinge ? Quaternion.Euler(0, isOpen ? 0 : -90, 0) : Quaternion.identity;

        while ((leftHinge && leftHinge.localRotation != leftTargetRotation) || (rightHinge && rightHinge.localRotation != rightTargetRotation))
        {
            if (leftHinge)
            {
                leftHinge.localRotation = Quaternion.RotateTowards(leftHinge.localRotation, leftTargetRotation, rotationSpeed * Time.deltaTime);
            }
            if (rightHinge)
            {
                rightHinge.localRotation = Quaternion.RotateTowards(rightHinge.localRotation, rightTargetRotation, rotationSpeed * Time.deltaTime);
            }
            yield return null;
        }

        isOpen = !isOpen;
        isAnimating = false;
    }
}
