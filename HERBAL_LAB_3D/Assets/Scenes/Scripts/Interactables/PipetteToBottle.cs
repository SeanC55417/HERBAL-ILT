using UnityEngine;
using LiquidVolumeFX;
using System.Runtime.InteropServices.WindowsRuntime;

public class PipetteToBottle : MonoBehaviour
{
    public GameObject bottleCap; // Reference to the bottle cap GameObject
    public PickupObject pickupObject; // Reference to the PickupObject script
    public GameObject pipette; // Reference to the specific pipette GameObject
    public LiquidVolume liquidVolume; // Reference to the LiquidVolume component on the bottle
    public float fillPipette = 0f; // Amount to fill the pipette
    public float fillBottle = 0f; // Amount to fill the bottle

    public enum Exchange { Fill, TransferTo, TransferFrom, Empty }
    public Exchange type = Exchange.Fill; // Type of liquid exchange

    private Renderer capRenderer;
    private bool isHovered = false;
    
    public bool pipetteToBottleActive = true;

    void Start()
    {
        // Get the Renderer component of the bottle cap
        if (bottleCap != null)
        {
            capRenderer = bottleCap.GetComponent<Renderer>();
        }
    }

    void Update()
    {
        if (pipetteToBottleActive)
        {
            HandlePipetteEquipped();
        }
    }

    // Handle the pipette being equipped and hovering detection
    void HandlePipetteEquipped()
    {
        if (pickupObject != null && pickupObject.GetObject() == pipette)
        {
            CheckHover();
        }
        else if (isHovered)
        {
            SetCapVisibility(true);
            isHovered = false;
        }
    }

    // Check if the mouse is hovering over the bottle
    void CheckHover()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (!isHovered) OnHoverEnter();

                if (Input.GetMouseButtonDown(0)) OnClicked();
            }
            else if (isHovered)
            {
                OnHoverExit();
            }
        }
        else if (isHovered)
        {
            OnHoverExit();
        }
    }

    // Handle actions when the mouse starts hovering over the bottle
    void OnHoverEnter()
    {
        isHovered = true;
        SetCapVisibility(false);
    }

    // Handle actions when the mouse stops hovering over the bottle
    void OnHoverExit()
    {
        isHovered = false;
        SetCapVisibility(true);
    }

    // Handle actions when the bottle is clicked
    void OnClicked()
    {
        if (liquidVolume != null)
        {
            LiquidVolume pipetteLiquid = pipette.GetComponentInChildren<LiquidVolume>();
            if (pipetteLiquid != null)
            {
                Debug.Log(liquidVolume.liquidLayers.Length);
                if (liquidVolume.detail == DETAIL.Multiple && liquidVolume.liquidLayers.Length > 1)
                {
                    // RemoveTopLayer();
                    MultiLayerLiquidExchange(pipetteLiquid);
                }
                else
                {
                    Debug.Log("Single");
                    PerformLiquidExchange(pipetteLiquid);
                }
            }
        }
    }

    // Perform liquid exchange based on the selected type
    void PerformLiquidExchange(LiquidVolume pipetteLiquid)
    {
        switch (type)
        {
            case Exchange.Fill:
                liquidVolume.level = fillBottle;
                pipetteLiquid.level = 0f;
                pipetteLiquid.liquidColor1 = liquidVolume.liquidColor1;
                pipetteLiquid.liquidColor2 = liquidVolume.liquidColor2;
                break;

            case Exchange.TransferTo:
                TransferLiquid(pipetteLiquid, fillPipette, fillBottle);
                break;

            case Exchange.TransferFrom:
                TransferLiquid(liquidVolume, fillBottle, fillPipette);
                break;

            case Exchange.Empty:
                pipetteLiquid.level = fillPipette;
                pipetteLiquid.liquidColor1 = liquidVolume.liquidColor1;
                pipetteLiquid.liquidColor2 = liquidVolume.liquidColor2;
                liquidVolume.level = 0f;
                break;
        }
    }

    // Handle multi-layer liquid exchange
    private void MultiLayerLiquidExchange(LiquidVolume pipetteLiquid)
    {
        int top = liquidVolume.liquidLayers.Length - 1;

        switch (type)
        {
            case Exchange.Fill:
                liquidVolume.liquidLayers[top].amount = fillBottle;
                pipetteLiquid.level = 0f;
                pipetteLiquid.liquidColor1 = liquidVolume.liquidLayers[top].color;
                pipetteLiquid.liquidColor2 = liquidVolume.liquidLayers[top].color;
                break;

            case Exchange.TransferTo:
                TransferLiquid(pipetteLiquid, fillPipette, fillBottle);
                break;

            case Exchange.TransferFrom:
                TransferLiquid(liquidVolume.liquidLayers[top], fillBottle, fillPipette);
                break;

            case Exchange.Empty:
                top = GetTopEmptyLayerIndex();
                pipetteLiquid.level = fillPipette;
                pipetteLiquid.liquidColor1 = liquidVolume.liquidLayers[top].color;
                pipetteLiquid.liquidColor2 = liquidVolume.liquidLayers[top].color;
                liquidVolume.liquidLayers[top].amount = 0f;
                break;
        }
        liquidVolume.UpdateLayers();
    }

    void RemoveTopLayer()
    {
        for(int i = liquidVolume.liquidLayers.Length - 1; i > -1; i--)
        {   
            if (liquidVolume.liquidLayers[i].amount != 0){
                liquidVolume.liquidLayers[i].amount = 0f;
                liquidVolume.UpdateLayers();
                return;
            }
        }
    }

    private int GetTopEmptyLayerIndex()
    {
        for (int i = liquidVolume.liquidLayers.Length - 1; i > -1; i--)
        {   
            if (liquidVolume.liquidLayers[i].amount != 0)
                return i;
        }
        return 0;
    }


    // Transfer liquid between two LiquidVolume components
    void TransferLiquid(LiquidVolume source, float sourceAmount, float targetAmount)
    {
        liquidVolume.level += targetAmount;
        source.level -= sourceAmount;

        liquidVolume.liquidColor1 = BlendColors(liquidVolume.liquidColor1, source.liquidColor1, sourceAmount);
        source.liquidColor1 = BlendColors(source.liquidColor1, liquidVolume.liquidColor1, targetAmount);

        liquidVolume.liquidColor2 = BlendColors(liquidVolume.liquidColor2, source.liquidColor2, sourceAmount);
        source.liquidColor2 = BlendColors(source.liquidColor2, liquidVolume.liquidColor2, targetAmount);
    }

        // Transfer liquid between two LiquidVolume components
    private void TransferLiquid(LiquidVolume.LiquidLayer source, float sourceAmount, float targetAmount)
    {
        liquidVolume.level += targetAmount;
        source.amount -= sourceAmount;

        liquidVolume.liquidColor1 = BlendColors(liquidVolume.liquidColor1, source.color, sourceAmount);
        source.color = BlendColors(source.color, liquidVolume.liquidColor1, targetAmount);

        liquidVolume.liquidColor2 = BlendColors(liquidVolume.liquidColor2, source.color, sourceAmount);
        source.color = BlendColors(source.color, liquidVolume.liquidColor2, targetAmount);
    }

    // Toggle the visibility of the bottle cap
    void SetCapVisibility(bool isVisible)
    {
        if (capRenderer != null)
        {
            capRenderer.enabled = isVisible;
        }
    }

    // Blend two colors based on a specified amount
    Color BlendColors(Color baseColor, Color mixColor, float amount)
    {
        return Color.Lerp(baseColor, mixColor, amount);
    }
}
