using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HplcSceneScript : MonoBehaviour
{
    public enum GameStep
    {
        OpenFireCabinet,
        BringWaterToHplc,
        BringMethanolToHplc,
        OpenHplcOven,
        OpenDrawer,
        PlaceColumnInHplc,
        CloseHplcOven,
        
        KnowledgeCheck1Q1,
        KnowledgeCheck1Q2,
        
        LoadToTray1,
        LoadToTray2,
        LoadToTray3,
        LoadToTray4,
        OpenHplcTraySlot,
        PlaceTrayInHplc,
        CloseHplcTraySlot,
        RunHplc,

        KnowledgeCheck2,
        KnowledgeCheck3,
        KnowledgeCheck4
    }
    
    private InstructionScript instruction;
    private GameStep currentStep;
    public TextMeshProUGUI hud_text;

    void Start()
    {
        // Find the instructionObject GameObject and get the InstructionScript component
        GameObject instructionObject = GameObject.Find("Instructions");

        if (instructionObject != null)
        {
            instruction = instructionObject.GetComponent<InstructionScript>();
        }

        // Ensure the script is found before trying to call its methods
        if (instruction != null)
        {
            Debug.Log("Instruction Script Found");
            instruction.postBulletPoints("Procedure", "Locate the solvent cabinet", "-Hint: flammable, yellow");
        }
        else
        {
            Debug.LogError("InstructionScript not found!");
        }

        // Initialize the first game step
        // currentStep = GameStep.OpenFireCabinet;
        currentStep = GameStep.CloseHplcOven;
    }

    void Update()
    {
        switch (currentStep)
        {
            case GameStep.OpenFireCabinet:
                if (IsFireCabinetOpened())
                {
                    CompleteStep(GameStep.BringWaterToHplc, "Bring the water to the HPLC");
                    instruction.postBulletPointWithTab("Select the polar solvent and bring over to the A slot in the rack atop the HPLC", "-Hint: aqueous");
                }
                break;
                
            case GameStep.BringWaterToHplc:
                if (IsRbObjectSet("Solvent Bottle Water"))
                {
                    CompleteStep(GameStep.BringMethanolToHplc, "Bring the methanol to the HPLC");
                    instruction.postBulletPointWithTab("Select the organic solvent from the cabinet and move to the B slot in the rack above the HPLC", "-Hint: CH3OH");
                }
                break;
            case GameStep.BringMethanolToHplc:
                if (IsRbObjectSet("Solvent Bottle Methanol"))
                {
                    CompleteStep(GameStep.OpenHplcOven, "Open the Hplc Oven");
                    instruction.postBulletPointWithTab("Equip the HPLC with the C18 column from the drawer below the computer", "-Hint: Pre-Column comes first");
                }
                break;
            case GameStep.OpenHplcOven:
                if (IsHplcOvenOpen())
                {
                    CompleteStep(GameStep.OpenDrawer, "Open the column drawer");
                }
                break;
            case GameStep.OpenDrawer:
                if (IsColumnDrawerOpen())
                {
                    CompleteStep(GameStep.PlaceColumnInHplc, "Place column into the HPLC");
                }
                break;
            case GameStep.PlaceColumnInHplc:
                if (IsRbObjectSet("ColumnRod"))
                {
                    CompleteStep(GameStep.CloseHplcOven, "Close HPLC oven");
                }
                break;
            case GameStep.CloseHplcOven:
                if (!IsHplcOvenOpen())
                {
                    CompleteStep(GameStep.KnowledgeCheck1Q1, "Press M to bring up Notebook to do KnowledgeCheck1");
                    instruction.postKnowledgeCheck("Question1");
                }
                break;

            case GameStep.KnowledgeCheck1Q1:
                if (instruction.getNumAnswered() > 0){
                    CompleteStep(GameStep.KnowledgeCheck1Q2, "Great Job!");
                    instruction.postKnowledgeCheck("Question2");
                }
                break;
            case GameStep.KnowledgeCheck1Q2:
                if (instruction.getNumAnswered() > 1){
                    CompleteStep(GameStep.LoadToTray1, "Add samples to tray (0/4)");
                }
                break;
            case GameStep.LoadToTray1:
                if(NumRbSetByTag("SampleBottle", 4) > 0)
                {
                    CompleteStep(GameStep.LoadToTray2, "Add samples to tray (1/4)");
                }
                break;
            case GameStep.LoadToTray2:
                if(NumRbSetByTag("SampleBottle", 4) > 1)
                {
                    CompleteStep(GameStep.LoadToTray3, "Add samples to tray (2/4)");
                }
                break;
            case GameStep.LoadToTray3:
                if(NumRbSetByTag("SampleBottle", 4) > 2)
                {
                    CompleteStep(GameStep.LoadToTray4, "Add samples to tray (3/4)");
                }
                break;
            case GameStep.LoadToTray4:
                if(NumRbSetByTag("SampleBottle", 4) > 3)
                {
                    CompleteStep(GameStep.OpenHplcTraySlot, "Open HPLC tray slot");
                }
                break;
            case GameStep.OpenHplcTraySlot:
                if(IsTraySlotOpen())
                {
                    CompleteStep(GameStep.PlaceTrayInHplc, "Place tray in HPLC");
                }
                break;
            case GameStep.PlaceTrayInHplc:
                if(IsRbObjectSet("Tray"))
                {
                    CompleteStep(GameStep.CloseHplcTraySlot, "Close HPLC tray slot");
                }
                break;
            case GameStep.CloseHplcTraySlot:
                if (!IsTraySlotOpen()) 
                {
                    CompleteStep(GameStep.RunHplc, "Run HPLC");
                }
                break;
            // Add more cases for each step as needed
        }
    }

    bool IsFireCabinetOpened()
    {
        // Check if the fire cabinet is opened
        GameObject fireCabinetDoor = GameObject.Find("Hinge_Left_FireCabinet");
        if (fireCabinetDoor != null)
        {
            float yRotation = fireCabinetDoor.transform.rotation.eulerAngles.y;

            if (yRotation > 45)
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("Fire Cabinet door not found!");
        }
        return false;
    }

    bool IsHplcOvenOpen()
    {
        GameObject hplcOvenDoor = GameObject.Find("Hplc Panel Hinge");
        if (hplcOvenDoor != null)
        {
            float yRotation = hplcOvenDoor.transform.localRotation.eulerAngles.y;
            // Yes this is strange, it is really just range of triggers for the door since rotations aren't precise
            if ((50 < yRotation && yRotation < 100) || (250 < yRotation && yRotation < 300))
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("Hplc oven door not found!");
        }
        return false;
    }

    bool IsColumnDrawerOpen()
    {
        GameObject columnDrawer = GameObject.Find("Column Drawer");
        if (columnDrawer != null)
        {
            float xPosition = columnDrawer.transform.localPosition.x;

            if (xPosition > -4.9f)
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("Column Drawer not found!");
        }
        return false;
    }

    bool IsTraySlotOpen()
    {
        GameObject columnDrawer = GameObject.Find("Tray Slot");
        if (columnDrawer != null)
        {
            float zPosition = columnDrawer.transform.localPosition.z;

            if (zPosition < -0.45f)
            {
                return true;
            }
        }
        else
        {
            Debug.LogError("Tray Slot not found!");
        }
        return false;
    }


    bool IsRbObjectSet(string objectName)
    {
        GameObject obj = GameObject.Find(objectName);
        if (obj != null)
        {   
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null)
            {
                return true; // Invariant if kinematic object is set
            }
        }
        return false;
    }

    int NumRbSetByTag(string objectTag, int total){
        int num = 0;
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(objectTag);
        foreach(GameObject obj in objectsWithTag){
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb == null){
                num += 1;
            }
        }
        return num;
    }

    void CompleteStep(GameStep nextStep, string instructionMessage)
    {
        Debug.Log($"Step {currentStep} completed. Proceed to: {nextStep}");
        currentStep = nextStep;

        // Post the next instruction
        if (instruction != null)
        {
            // instruction.postBulletPoints("Procedure", instructionMessage);
            hud_text.text = instructionMessage;
        }
    }
}
