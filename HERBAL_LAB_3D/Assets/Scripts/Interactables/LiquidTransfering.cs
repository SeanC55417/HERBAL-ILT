using System;
using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using Unity.VisualScripting;
using UnityEngine;

public class LiquidTransfering : MonoBehaviour
{
    public Renderer cap;
    public GameObject container;
    public GameObject pipette;
    public float amountPipette;
    public float amountContainer;
    public bool liquidTransferingActive = true;
    
    private PickupObject pickupObject;
    private LiquidVolume liquidVolume;
    private (Color, Color) pipetteColor;
    private readonly float bufferTime = 5f;
    private bool bufferCoroutineRunning = false;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        pickupObject = FindAnyObjectByType<PickupObject>();
        liquidVolume = container.GetComponent<LiquidVolume>();

        gameManager = FindAnyObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (liquidTransferingActive){
            CheckHoveringContainer();
        }
    }

    void CheckHoveringContainer(){
        // Cast a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Check if the ray hits an object with a collider
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
           // If the hit object is the specific GameObject
            if (hit.collider.gameObject == container && pickupObject.heldObject == pipette)
            {
                cap.enabled = false;
                if (Input.GetMouseButtonDown(0)) OnClicked();
                return;
            }
        }
        cap.enabled = true;
    }

  void OnTriggerEnter(Collider other)
    {
       if (liquidTransferingActive && other.gameObject == pipette && gameManager.IsVR)
       {
            liquidTransferingActive = false;
            OnClicked();
       }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == pipette && !bufferCoroutineRunning)
        {
            liquidTransferingActive = false;  // Disable immediately on exit
            StartCoroutine(BufferCoroutine());
        }
    }

    private IEnumerator BufferCoroutine()
    {
        bufferCoroutineRunning = true;  // Prevent multiple coroutines from running
        yield return new WaitForSeconds(bufferTime);
        liquidTransferingActive = true;
        bufferCoroutineRunning = false;
    }

    public void OnClicked(){
        
        if (liquidVolume.detail == DETAIL.Multiple){
            MultiLayerTransfer();
        }
        else{
            SingleLayerTransfer();
        }
        LiquidVolume pipetteLiquid = pipette.GetComponentInChildren<LiquidVolume>();
        pipetteLiquid.liquidColor1 = pipetteColor.Item1;
        pipetteLiquid.liquidColor2 = pipetteColor.Item2;
        pipetteLiquid.level = amountPipette;
    }

    void MultiLayerTransfer()
    {
        int idx = TopLayerIndex();
        liquidVolume.liquidLayers[idx].amount = amountContainer;
        pipetteColor = (liquidVolume.liquidLayers[idx].color, liquidVolume.liquidLayers[idx].color);
        float scaleRatio = 1 / liquidVolume.liquidScale1 + liquidVolume.liquidScale2;
        liquidVolume.liquidLayers[idx].color = BlendColors(liquidVolume.liquidLayers[idx].color, BlendColors(liquidVolume.liquidColor1, liquidVolume.liquidColor2, scaleRatio), amountPipette);
        liquidVolume.speed = 1; // testomg fp s[eed]
        liquidVolume.UpdateLayers();
    }

    void SingleLayerTransfer()
    {
        liquidVolume.level = amountContainer;
        pipetteColor = (liquidVolume.liquidColor1, liquidVolume.liquidColor2);
        liquidVolume.liquidColor1 = BlendColors(liquidVolume.liquidColor1, pipetteColor.Item1, amountPipette);
        liquidVolume.liquidColor2 = BlendColors(liquidVolume.liquidColor2, pipetteColor.Item2, amountPipette);
    }
    
    int TopLayerIndex()
    {
        for (int i = liquidVolume.liquidLayers.Length - 1; i > -1; i--)
        {   
            if (liquidVolume.liquidLayers[i].amount != 0)
                return i;
        }
        return 0;
    }

    Color BlendColors(Color baseColor, Color mixColor, float amount)
    {
        return Color.Lerp(baseColor, mixColor, amount);
    }

    public float GetContainerLevel()
    {
        if (liquidVolume.detail == DETAIL.Multiple){
            return liquidVolume.liquidLayers[TopLayerIndex()].amount;
        }
        return liquidVolume.level;
    }

    public void SetPipette(GameObject newPipette)
    {
        pipette = newPipette;
    }
}
