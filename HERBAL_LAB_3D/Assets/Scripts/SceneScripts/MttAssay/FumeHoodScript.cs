using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FumeHoodScript : MonoBehaviour
{
    public GameObject trypsin;
    public GameObject medium;
    public MttAssaySceneScript sceneScript;

    private bool trypsinInside = false;
    private bool mediumInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == trypsin)
        {
            trypsinInside = true;
        }

        if (other.gameObject == medium)
        {
            mediumInside = true;
        }
        CheckIfBothInside();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == trypsin)
        {
            trypsinInside = false;
        }

        if (other.gameObject == medium)
        {
            mediumInside = false;
        }
        CheckIfBothInside();
    }

    public void CheckIfBothInside()
    {
        if (trypsinInside && mediumInside)
        {
            sceneScript.SetTrypsinAndMediumInFumeHood(true);
        }
        else
        {
            sceneScript.SetTrypsinAndMediumInFumeHood(false);
        }
    }
}
