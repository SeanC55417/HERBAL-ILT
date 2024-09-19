using System.Collections;
using UnityEngine;
using LiquidVolumeFX;

public class TrashPipette : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject pipette;
    public PickupObject pickupObject;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    public void Trash()
    {
        if (pickupObject == null)
        {
            Debug.LogWarning("PickupObject component not found on this GameObject.");
            return;
        }

        if (targetObject == null || pipette == null)
        {
            Debug.LogWarning("Target object or pipette is not assigned.");
            return;
        }

        GameObject heldObject = pickupObject.GetHeldObject();
        if (heldObject == targetObject)
        {
           RemovePipette();
        }
        else
        {
            Debug.Log("Held object is not the target object. No action taken.");
        }
    }

    private void RemovePipette()
    {
        // Null check for LiquidVolume before accessing
        LiquidVolume liquidVolume = pipette.GetComponent<LiquidVolume>();
        if (liquidVolume != null)
        {
            liquidVolume.level = 0.0f;
        }

        // Start the coroutine to make the pipette invisible
        StartCoroutine(MakePipetteInvisible());
    }

    private IEnumerator MakePipetteInvisible()
    {
        
        if (pipette.TryGetComponent<Renderer>(out var pipetteRenderer))
        {
            pipetteRenderer.enabled = false;
            Debug.Log("Pipette made invisible.");
        }
        else
        {
            Debug.LogWarning("Renderer not found on pipette. Cannot make it invisible.");
        }

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Re-enable the renderer
        if (pipetteRenderer != null)
        {
            pipetteRenderer.enabled = true;
            Debug.Log("Pipette made visible again.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetObject && gameManager != null && gameManager.IsVR)
        {
            RemovePipette();
        }
    }
}
