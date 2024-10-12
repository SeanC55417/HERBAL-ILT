using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeController : MonoBehaviour
{
    public enum HingeDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    [System.Serializable]
    public class Hinge
    {
        public Transform hingeTransform;
        public HingeDirection direction;
        public float openAngle = 90f; // The target angle when the hinge is fully open
        public float closeAngle = 0f; // The target angle when the hinge is fully closed
    }

    public List<Hinge> hinges = new List<Hinge>();
    public float rotationSpeed = 90f; // degrees per second

    private bool isOpen = false;
    private bool isAnimating = false; // To track if the animation is playing

    public void ToggleHinges()
    {
        if (!isAnimating) // Only allow toggling if not currently animating
        {
            StartCoroutine(AnimateHinges());
        }
    }

    private IEnumerator AnimateHinges()
    {
        isAnimating = true;

        // Determine target rotation for each hinge
        List<Quaternion> targetRotations = new List<Quaternion>();
        foreach (Hinge hinge in hinges)
        {
            if (hinge.hingeTransform != null)
            {
                float targetAngle = isOpen ? hinge.closeAngle : hinge.openAngle;
                Vector3 hingeAxis = GetHingeAxis(hinge.direction);
                targetRotations.Add(Quaternion.Euler(hingeAxis * targetAngle));
            }
            else
            {
                targetRotations.Add(Quaternion.identity);
            }
        }

        // Animate each hinge until all are at the target rotation
        bool allHingesAtTarget = false;
        while (!allHingesAtTarget)
        {
            allHingesAtTarget = true;
            for (int i = 0; i < hinges.Count; i++)
            {
                if (hinges[i].hingeTransform != null)
                {
                    hinges[i].hingeTransform.localRotation = Quaternion.RotateTowards(hinges[i].hingeTransform.localRotation, targetRotations[i], rotationSpeed * Time.deltaTime);
                    if (hinges[i].hingeTransform.localRotation != targetRotations[i])
                    {
                        allHingesAtTarget = false;
                    }
                }
            }
            yield return null;
        }

        isOpen = !isOpen;
        isAnimating = false;
    }

    private Vector3 GetHingeAxis(HingeDirection direction)
    {
        switch (direction)
        {
            case HingeDirection.Left:
                return Vector3.up;
            case HingeDirection.Right:
                return Vector3.down;
            case HingeDirection.Up:
                return Vector3.left;
            case HingeDirection.Down:
                return Vector3.right;
            default:
                return Vector3.up;
        }
    }
}
