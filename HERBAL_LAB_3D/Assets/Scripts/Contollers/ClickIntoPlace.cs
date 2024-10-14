using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickIntoPlace : MonoBehaviour
{
    public void clickInPlace(GameObject destinationParent)
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        Destroy(rb);

        Collider collider = gameObject.GetComponent<Collider>();            
        collider.enabled = false;

        gameObject.transform.SetParent(destinationParent.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
