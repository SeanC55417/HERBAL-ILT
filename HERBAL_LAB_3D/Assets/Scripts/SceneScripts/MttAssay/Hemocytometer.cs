using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using LiquidVolumeFX;
using Unity.VisualScripting;
using UnityEngine;

public class Hemocytometer : MonoBehaviour
{

    public PickupObject pickupObject;
    public GameObject targetPipette;
    public GameObject fillArea;
    private bool filled = false;
    private bool hemocytometerActive = false;

    public bool Filled
    {
        get { return filled; }
        set { filled = value; }
    }
    public bool HemocytometerActive
    {
        get { return hemocytometerActive; }
        set { hemocytometerActive = value; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hemocytometerActive)
        {
            
        }
    }

    public void CheckPipette()
    {
        if (pickupObject.GetHeldObject() == targetPipette)
        {
            
            if (targetPipette.TryGetComponent<LiquidVolume>(out var liquidVolume))
            {
                liquidVolume.level = 0.0f;
            }
            fillArea.SetActive(true);
        }
    }
}
