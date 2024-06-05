using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using System.Collections.Specialized;

public class BatchMode : MonoBehaviour
{
    public Button BatchButton;
    public Button addBatchTable;
    public Button selection2;
    public Button selection3;
    public Button selection4;

    public GameObject Screen;
    public Material image2;
    public Material image3;
    public Material image4;
    public Material image5;
    public Material image6;
    public Material image7;
    public Material image8;
    public GameObject startBatch;

    private int iterator = 1;

    public void batchMenuSpawn()
    {
        BatchButton.transform.localPosition = new Vector3(-247, 33, -1000);
        addBatchTable.transform.localPosition = new Vector3(-3, -96, 0);
    }

    public void startRunning()
    {
        startBatch.SetActive(true);
    }

    public void makeBarkExtractAppear()
    {
        if (iterator == 2)
        {
            selection2.transform.localPosition = new Vector3(-138, 0, -1000);
            Screen.GetComponent<MeshRenderer>().material = image3;
            iterator = 3;
            addBatchTable.transform.localPosition = new Vector3(-3, -96, 0);
        }
    }

    public void makeLeavesExtractAppear()
    {
        if (iterator == 4)
        {
            selection3.transform.localPosition = new Vector3(-138, 40, -1000);
            Screen.GetComponent<MeshRenderer>().material = image5;
            iterator = 5;
            addBatchTable.transform.localPosition = new Vector3(-3, -96, 0);
        }
    }

    public void makeFruitExtractAppear()
    {
        if (iterator == 6)
        {
            selection4.transform.localPosition = new Vector3(-138, 80, -1000);
            Screen.GetComponent<MeshRenderer>().material = image7;
            iterator = 7;
            addBatchTable.transform.localPosition = new Vector3(-3, -96, 0);
        }
    }

    public void addBatch()
    {
        BatchButton.transform.localPosition = new Vector3(0, 0, -1000);
        addBatchTable.transform.localPosition = new Vector3(0, 0, -1000);
        if (iterator == 3)
        {
            Screen.GetComponent<MeshRenderer>().material = image4;
            selection3.transform.localPosition = new Vector3(-138, 40, 0);
            iterator = 4;
        }
        else if (iterator == 5)
        {
            Screen.GetComponent<MeshRenderer>().material = image6;
            selection4.transform.localPosition = new Vector3(-138, 80, 0);
            iterator = 6;
        }
        else if (iterator == 7)
        {
            Screen.GetComponent<MeshRenderer>().material = image8;
            iterator = 8;
        }
        else 
        {
            Screen.GetComponent<MeshRenderer>().material = image2;
            selection2.transform.localPosition = new Vector3(-138, 0, 0);
            iterator = 2;
        }
    }

    public void AutoComplete()
    {
        StartCoroutine(AutoCompleteCoroutine());
    }

    private IEnumerator AutoCompleteCoroutine()
   {
        for (int i = 2; i <= 8; i += 1) // Start at image2 and iterate up to image8
        {
            // Set the screen material to the appropriate image
            switch (i)
            {
                case 2:
                    Screen.GetComponent<MeshRenderer>().material = image2;
                    break;
                case 3:
                    Screen.GetComponent<MeshRenderer>().material = image3;
                    break;
                case 4:
                    Screen.GetComponent<MeshRenderer>().material = image4;
                    break;
                case 5:
                    Screen.GetComponent<MeshRenderer>().material = image5;
                    break;
                case 6:
                    Screen.GetComponent<MeshRenderer>().material = image6;
                    break;
                case 7:
                    Screen.GetComponent<MeshRenderer>().material = image7;
                    break;
                case 8:
                    Screen.GetComponent<MeshRenderer>().material = image8;
                    break;
            }

            // Wait for 0.2 seconds before changing to the next image
            yield return new WaitForSeconds(0.2f);
        }


    }
}
