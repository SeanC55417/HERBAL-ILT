using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class TrashPipette : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject pipette;
    public PickupObject pickupObject;

    public void Trash()
    {
        if (pickupObject == null)
        {
            Debug.LogWarning("PickupObject component not found on this GameObject.");
            return;
        }


        if (pickupObject.GetHeldObject() == targetObject)
        {
            // Perform a null check before assignment
            LiquidVolume liquidVolume = pipette.GetComponent<LiquidVolume>();
            if (liquidVolume != null)
            {
                liquidVolume.level = 0.0f;
            }

            // Start the coroutine to make the pipette invisible
            StartCoroutine(MakePipetteInvisible());
        }
        else
        {
            Debug.Log("Held object is not the target object. No action taken.");
        }
    }

    private IEnumerator MakePipetteInvisible()
    {
        // Disable the renderer to make the pipette invisible
        Renderer pipetteRenderer = pipette.GetComponent<Renderer>();
        if (pipetteRenderer != null)
        {
            pipetteRenderer.enabled = false;
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Re-enable the renderer to make the pipette visible again
        if (pipetteRenderer != null)
        {
            pipetteRenderer.enabled = true;
        }
    }
}
