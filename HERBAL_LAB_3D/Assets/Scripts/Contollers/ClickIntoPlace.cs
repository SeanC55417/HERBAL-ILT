using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickIntoPlace : MonoBehaviour
{
    public void clickInPlace(GameObject destinationParent)
    {
        GameObject XROrigin = GameObject.Find("XR Origin (XR Rig) Variant");

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null && XROrigin && XROrigin.activeSelf) {
            Debug.Log("Destorying rb");
            Destroy(rb);
        }
        

        Collider collider = gameObject.GetComponent<Collider>();            
        collider.enabled = false;

        gameObject.transform.SetParent(destinationParent.transform);
        gameObject.transform.localPosition = Vector3.zero;
    }
}
