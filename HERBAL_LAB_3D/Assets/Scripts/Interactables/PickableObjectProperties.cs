using UnityEngine;

public class PickableObjectProperties : MonoBehaviour
{
    public Vector3 preferredHeldPosition = Vector3.zero; // Local position relative to the hand
    public Quaternion preferredHeldRotation = Quaternion.identity; // Local rotation relative to the hand
}
