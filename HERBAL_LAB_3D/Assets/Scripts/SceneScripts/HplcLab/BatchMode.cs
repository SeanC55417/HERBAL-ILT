using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatchMode : MonoBehaviour
{
    public List<Sprite> images = new List<Sprite>();
    private int currentImageInt = 0;
    private Image batchImage;
    private Sprite nextBatchToAdd;
    bool addBatchBool = true;

    void Start()
    {
        Debug.Log("START FUNCTION RUNNING IN BATCHMODE");
        batchImage = gameObject.GetComponent<Image>();
        if (images[1] != null)
        {
            nextBatchToAdd = images[1];
        }
        else
        {
            Debug.Log("image count: " + images.Count);
        }
        
        if(batchImage == null)
        {
            Debug.LogError("Image component is not attached to the game object");
        }
    }

    public void Test()
    {
        Debug.Log("running");
    }

    public void makeBarkExtractAppear()
    {
        if (images.Count >= 4 && !addBatchBool)
        {
            batchImage.sprite = images[2];
            nextBatchToAdd = images[3];
            addBatchBool = true;
        }
        else
        {
            Debug.LogWarning("Not enough images in the list for Bark Extract.");
        }
    }

    public void makeLeavesExtractAppear()
    {
        if (images.Count >= 6 && !addBatchBool)
        {
            batchImage.sprite = images[4];
            nextBatchToAdd = images[5];
            addBatchBool = true;
        }
        else
        {
            Debug.LogWarning("Not enough images in the list for Leaves Extract.");
        }
    }

    public void makeFruitExtractAppear()
    {
        if (images.Count >= 8 && !addBatchBool)
        {
            batchImage.sprite = images[6];
            nextBatchToAdd = images[7];
            addBatchBool = true;
        }
        else
        {
            Debug.LogWarning("Not enough images in the list for Fruit Extract.");
        }
    }

    public void addBatch()
    {
        if (nextBatchToAdd != null && addBatchBool)
        {
            batchImage.sprite = nextBatchToAdd;
            addBatchBool = false;
        }
        else
        {
            Debug.LogWarning("Next batch to add is null.");
        }
    }

    public void AutoComplete()
    {
        Debug.Log("THIS SHOULD BE RUNNING");
        if (batchImage == null)
        {
            batchImage = gameObject.GetComponent<Image>();
        }
        StopAllCoroutines(); // Ensure any running coroutine is stopped before starting a new one
        StartCoroutine(AutoCompleteCoroutine());
    }

    private IEnumerator AutoCompleteCoroutine()
    {
        for (int i = 0; i < images.Count; i++)
        {
            batchImage.sprite = images[i];
            yield return new WaitForSeconds(0.2f);
            currentImageInt++;
        }
    }
}
