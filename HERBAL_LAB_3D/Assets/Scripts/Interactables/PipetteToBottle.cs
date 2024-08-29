using UnityEngine;
using LiquidVolumeFX;
using System;

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
        HandlePipetteEquipped();
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
                if (liquidVolume.liquidLayers.Length > 1 || pipetteLiquid.liquidLayers.Length > 1)
                {
                    MultiLayerLiquidExchange(pipetteLiquid.liquidLayers);
                }
                else
                {
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
    private void MultiLayerLiquidExchange(LiquidVolume.LiquidLayer[] liquidLayers)
    {
        // LiquidVolumeLayer[] bottleLayers = pipetteLayers;
        // switch (type)
        // {
        //     case Exchange.Fill:
        //         // Empty the pipette and fill the bottle with pipette's liquid layers
        //         for (int i = 0; i < bottleLayers.Length && i < pipetteLayers.Length; i++)
        //         {
        //             bottleLayers[i].layer.amount = pipetteLayers[i].amount;
        //             bottleLayers[i].color = pipetteLayers[i].color;
        //             pipetteLayers[i].amount = 0f; // Clear pipette layer after transfer
        //         }
        //         liquidVolume.UpdateLayers();
        //         break;

        //     case Exchange.TransferTo:
        //         // Transfer liquid from pipette to bottle
        //         for (int i = 0; i < bottleLayers.Length && i < pipetteLayers.Length; i++)
        //         {
        //             float transferAmount = Mathf.Min(pipetteLayers[i].amount, fillPipette);
        //             bottleLayers[i].amount += transferAmount;
        //             pipetteLayers[i].amount -= transferAmount;

        //             // Blend colors proportionally
        //             bottleLayers[i].color = BlendColors(bottleLayers[i].color, pipetteLayers[i].color, transferAmount);
        //         }
        //         liquidVolume.UpdateLayers();
        //         break;

        //     case Exchange.TransferFrom:
        //         // Transfer liquid from bottle to pipette
        //         for (int i = 0; i < bottleLayers.Length && i < pipetteLayers.Length; i++)
        //         {
        //             float transferAmount = Mathf.Min(bottleLayers[i].amount, fillBottle);
        //             pipetteLayers[i].amount += transferAmount;
        //             bottleLayers[i].amount -= transferAmount;

        //             // Blend colors proportionally
        //             pipetteLayers[i].color = BlendColors(pipetteLayers[i].color, bottleLayers[i].color, transferAmount);
        //         }
        //         liquidVolume.UpdateLayers();
        //         break;

        //     case Exchange.Empty:
        //         // Empty the bottle and fill the pipette with bottle's liquid layers
        //         for (int i = 0; i < pipetteLayers.Length && i < bottleLayers.Length; i++)
        //         {
        //             pipetteLayers[i].amount = bottleLayers[i].amount;
        //             pipetteLayers[i].color = bottleLayers[i].color;
        //             bottleLayers[i].amount = 0f; // Clear bottle layer after transfer
        //         }
        //         liquidVolume.UpdateLayers();
        //         break;
        // }
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
