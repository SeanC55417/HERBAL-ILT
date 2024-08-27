using UnityEngine;
using LiquidVolumeFX;

public class PipetteToBottle : MonoBehaviour
{
    public GameObject bottleCap; // Reference to the bottle cap GameObject
    public PickupObject pickupObject; // Reference to the PickupObject script
    public GameObject pipette; // Reference to the specific pipette GameObject
    public LiquidVolume liquidVolume; // Reference to the LiquidVolume component on the bottle
    public float fillPipette = 0f;
    public float fillBottle = 0f;
    public enum Exchange { Fill, TransferTo, TransferFrom, Empty };
    public Exchange type = Exchange.Fill;

    private Renderer capRenderer;
    private bool isHovered = false;

    void Start()
    {
        // Get the Renderer component of the bottle cap
        if (bottleCap != null)
        {
            capRenderer = bottleCap.GetComponent<Renderer>();
        }
        else
        {
            Debug.LogError("Bottle cap not assigned!");
        }

        // Ensure the PickupObject script reference is assigned
        if (pickupObject == null)
        {
            Debug.LogError("PickupObject script reference not assigned!");
        }

        // Ensure the pipette reference is assigned
        if (pipette == null)
        {
            Debug.LogError("Pipette object reference not assigned!");
        }

        // Ensure the LiquidVolume reference is assigned
        if (liquidVolume == null)
        {
            Debug.LogError("LiquidVolume component not assigned!");
        }
    }

    void Update()
    {
        // Check if the pipette is equipped
        if (pickupObject != null && pickupObject.GetObject() == pipette)
        {
            // Check if the object is being hovered over
            CheckHover();
        }
        else
        {
            if (isHovered)
            {
                // If the pipette is no longer equipped, ensure the cap is visible
                SetCapVisibility(true);
                isHovered = false;
            }
        }
    }

    void CheckHover()
    {
        // Perform a raycast from the camera to detect if hovering over the bottle, ignoring trigger colliders
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Use QueryTriggerInteraction.Ignore to ensure it ignores trigger colliders
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (!isHovered)
                {
                    OnHoverEnter();
                }

                // Detect a click event
                if (Input.GetMouseButtonDown(0))
                {
                    OnClicked();
                }
            }
            else
            {
                if (isHovered)
                {
                    OnHoverExit();
                }
            }
        }
        else
        {
            if (isHovered)
            {
                OnHoverExit();
            }
        }
    }

    void OnHoverEnter()
    {
        isHovered = true;
        // Make the bottle cap disappear
        SetCapVisibility(false);
    }

    void OnHoverExit()
    {
        isHovered = false;
        // Make the bottle cap reappear
        SetCapVisibility(true);
    }

    void OnClicked()
    {
        // if (liquidVolume.liquidLayers.Length > 1){
        //     liquidVolume.liquidLayers[0];
        // }

        if (liquidVolume != null)
        {
            LiquidVolume pipetteLiquid = pipette.GetComponentInChildren<LiquidVolume>();

            if (pipetteLiquid != null)
            {
                switch (type)
                {
                    case Exchange.Fill:
                        liquidVolume.level = fillBottle;
                        pipetteLiquid.level = 0f;
                        pipetteLiquid.liquidColor1 = liquidVolume.liquidColor1; // Transfer color1
                        pipetteLiquid.liquidColor2 = liquidVolume.liquidColor2; // Transfer color2
                        break;

                    case Exchange.TransferTo:
                        // Transfer liquid from pipette to bottle
                        liquidVolume.level += fillBottle;
                        pipetteLiquid.level -= fillPipette;

                        // Handle liquidColor1
                        if (pipetteLiquid.level <= 0f)
                        {
                            liquidVolume.liquidColor1 = pipetteLiquid.liquidColor1;
                        }
                        else if (liquidVolume.level <= 0f)
                        {
                            pipetteLiquid.liquidColor1 = liquidVolume.liquidColor1;
                        }
                        else
                        {
                            liquidVolume.liquidColor1 = BlendColors(liquidVolume.liquidColor1, pipetteLiquid.liquidColor1, fillPipette);
                            pipetteLiquid.liquidColor1 = BlendColors(pipetteLiquid.liquidColor1, liquidVolume.liquidColor1, fillBottle);
                        }

                        // Handle liquidColor2
                        if (pipetteLiquid.level <= 0f)
                        {
                            liquidVolume.liquidColor2 = pipetteLiquid.liquidColor2;
                        }
                        else if (liquidVolume.level <= 0f)
                        {
                            pipetteLiquid.liquidColor2 = liquidVolume.liquidColor2;
                        }
                        else
                        {
                            liquidVolume.liquidColor2 = BlendColors(liquidVolume.liquidColor2, pipetteLiquid.liquidColor2, fillPipette);
                            pipetteLiquid.liquidColor2 = BlendColors(pipetteLiquid.liquidColor2, liquidVolume.liquidColor2, fillBottle);
                        }
                        break;

                    case Exchange.TransferFrom:
                        // Transfer liquid from bottle to pipette
                        liquidVolume.level -= fillBottle;
                        pipetteLiquid.level += fillPipette;

                        // Handle liquidColor1
                        if (pipetteLiquid.level <= 0f)
                        {
                            liquidVolume.liquidColor1 = pipetteLiquid.liquidColor1;
                        }
                        else if (liquidVolume.level <= 0f)
                        {
                            pipetteLiquid.liquidColor1 = liquidVolume.liquidColor1;
                        }
                        else
                        {
                            liquidVolume.liquidColor1 = BlendColors(liquidVolume.liquidColor1, pipetteLiquid.liquidColor1, fillPipette);
                            pipetteLiquid.liquidColor1 = BlendColors(pipetteLiquid.liquidColor1, liquidVolume.liquidColor1, fillBottle);
                        }

                        // Handle liquidColor2
                        if (pipetteLiquid.level <= 0f)
                        {
                            liquidVolume.liquidColor2 = pipetteLiquid.liquidColor2;
                        }
                        else if (liquidVolume.level <= 0f)
                        {
                            pipetteLiquid.liquidColor2 = liquidVolume.liquidColor2;
                        }
                        else
                        {
                            liquidVolume.liquidColor2 = BlendColors(liquidVolume.liquidColor2, pipetteLiquid.liquidColor2, fillPipette);
                            pipetteLiquid.liquidColor2 = BlendColors(pipetteLiquid.liquidColor2, liquidVolume.liquidColor2, fillBottle);
                        }
                        break;

                    case Exchange.Empty:
                        if (pipetteLiquid != null)
                        {
                            pipetteLiquid.level = fillPipette;
                            pipetteLiquid.liquidColor1 = liquidVolume.liquidColor1; // Transfer color1
                            pipetteLiquid.liquidColor2 = liquidVolume.liquidColor2; // Transfer color2
                        }
                        liquidVolume.level = 0f;
                        break;
                }
            }
        }
    }

    void SetCapVisibility(bool isVisible)
    {
        if (capRenderer != null)
        {
            capRenderer.enabled = isVisible;
        }
    }

    Color BlendColors(Color baseColor, Color mixColor, float amount)
    {
        return Color.Lerp(baseColor, mixColor, amount);
    }
}
