using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using UnityEngine;

public class VolumeChange : MonoBehaviour
{
    private bool isClean = false;
    private bool hasTrypsin = false;
    private bool hasMedium = false;
    public GameObject liquidArea;
    private LiquidVolume liquidVolume;
    private float liquidAmount;
    private int change = 0;
    // Start is called before the first frame update
    void Start()
    {
        liquidVolume = liquidArea.GetComponent<LiquidVolume>();
        liquidAmount = liquidVolume.level;
    }

    // Update is called once per frame
    void Update()
    {
        if (liquidAmount != liquidVolume.level)
        {
            liquidAmount = liquidVolume.level;
                switch (change){
                case 0:
                    isClean = true;
                break;
                case 1:
                    hasTrypsin = true;
                break;
                case 2:
                    hasMedium = true;
                break;
                default:
                break;
            }
            change++;
            
        }
    }

        // Getter method to report if the object has been shaken

    public bool IsClean()
    {
        return isClean;
    }
    public bool HasTrypsin()
    {
        return hasTrypsin;
    }
    public bool HasMedium()
    {
        return hasMedium;
    }
}
