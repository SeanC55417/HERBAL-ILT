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

        EnterMethod,

        KnowledgeCheck2Q1,
        KnowledgeCheck2Q2,
        
        SelectPump,
        SelectQuickBatch,
        AssignSamples,
        RunHplc,

        KnowledgeCheck3Q1,
        KnowledgeCheck3Q2,
    
        KnowledgeCheck4Q1,
        KnowledgeCheck4Q2,

        Done
    }
    
    private InstructionScript instruction;
    private NotebookScript notebook;
    private GameStep currentStep;
    private GameObject computerScreen = null;

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

        GameObject notebookObject = GameObject.Find("Notebook");

        if (notebookObject != null)
        {
            notebook = notebookObject.GetComponent<NotebookScript>();
        }

        // Ensure the script is found before trying to call its methods
        if (notebook != null)
        {
            Debug.Log("Notebook Script Found");
            notebook.postText("Instrument Parameters");
        }
        else
        {
            Debug.LogError("NotebookScript not found!");
        }

        // Initialize the first game step
        currentStep = GameStep.OpenFireCabinet;

        hud_text.text = "Welcome to the HplcLab!\nOpen the fire cabinet to begin lab";

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
                    notebook.postText("\nPolar Sol. A: Water");
                }
                break;
            case GameStep.BringMethanolToHplc:
                if (IsRbObjectSet("Solvent Bottle Methanol"))
                {
                    CompleteStep(GameStep.OpenHplcOven, "Open the Hplc Oven");
                    instruction.postBulletPointWithTab("Equip the HPLC with the C18 column from the drawer below the computer", "-Hint: Pre-Column comes first");
                    notebook.postText("\nOrganic Sol. B: Methanol");
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
                    notebook.postText("\nColumn: Phenomenex Kinetex C18 (4.6 x 150 mm) 100 Å");
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
                    CompleteStep(GameStep.EnterMethod, "Enter method on computer");
                }
                break;
            case GameStep.EnterMethod:
                if (ChildrenInParent("PC Screen Menu Mode Canvas") == 2){
                    CompleteStep(GameStep.KnowledgeCheck2Q1, "Press M to bring up the notebook to do KnowledgeCheck2");
                    instruction.postKnowledgeCheck("Question3");
                }
                break;
            case GameStep.KnowledgeCheck2Q1:
                int answered = instruction.getNumAnswered();
                if (instruction.getNumAnswered() > 2){
                    CompleteStep(GameStep.KnowledgeCheck2Q2, "");
                    instruction.postKnowledgeCheck("Question4");
                }
                break;
            case GameStep.KnowledgeCheck2Q2:
                if (instruction.getNumAnswered() > 3){
                    CompleteStep(GameStep.SelectPump, "Awesome!\nBack onto the computer click the \"Pump ON\" button");
                    GameObject pumpButton = GameObject.Find("Pump Button");
                    Collider collider = pumpButton.GetComponent<Collider>();
                    collider.enabled = true;
                }
                break;
            case GameStep.SelectPump:
                GameObject pumpRunningCanvas;
                pumpRunningCanvas = GameObject.Find("Pump Running Canvas");

                if (pumpRunningCanvas != null){
                    CompleteStep(GameStep.SelectQuickBatch, "Select \"Quick Batch\" on the computer");
                }
                break;
            case GameStep.SelectQuickBatch:
                computerScreen = GameObject.Find("Screen");
                if (computerScreen != null)
                {
                    GameObject batchScreen = GameObject.Find("PC Screen Batch Mode Canvas");
                    if (batchScreen != null)
                    {
                        CompleteStep(GameStep.AssignSamples, "Assign samples (Optional for now) then click Start");
                    }
                }
                break;
            case GameStep.AssignSamples:
                computerScreen = GameObject.Find("Screen");
                if (computerScreen != null)
                {
                    MeshRenderer meshRenderer = computerScreen.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        Material material = meshRenderer.material;
                        if (material != null && material.name == "8 (Instance)")
                        {
                            CompleteStep(GameStep.RunHplc, "Run HPLC");
                        }
                    }
                }
                break;
            case GameStep.RunHplc:
                GameObject allGraphsScreen = GameObject.Find("PC Screen Graphs");
                if (allGraphsScreen != null){
                    CompleteStep(GameStep.KnowledgeCheck3Q1, "Press M to bring up the notebook to do KnowledgeCheck3");
                    instruction.postKnowledgeCheck("Question5");
                }
                break;
            case GameStep.KnowledgeCheck3Q1:
                if (instruction.getNumAnswered() > 4){
                    CompleteStep(GameStep.KnowledgeCheck3Q2, "");
                    instruction.postKnowledgeCheck("Question6");
                }
                break;
            case GameStep.KnowledgeCheck3Q2:
                if (instruction.getNumAnswered() > 5){
                    CompleteStep(GameStep.KnowledgeCheck4Q1, "Use the graphs for the next set of questions");
                    instruction.postKnowledgeCheck("Question7");
                }
                break;
            case GameStep.KnowledgeCheck4Q1:
                if (instruction.getNumAnswered() > 6){
                    CompleteStep(GameStep.KnowledgeCheck4Q2, "");
                    instruction.postKnowledgeCheck("Question8");
                }
                break;
            case GameStep.KnowledgeCheck4Q2:
                if (instruction.getNumAnswered() > 7){
                    CompleteStep(GameStep.Done, "Congratulations you finished the module, use the notebook to exit the module");
                }
                break;
            case GameStep.Done:
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

    int ChildrenInParent(string objectName)
    {
        GameObject parentObject = GameObject.Find(objectName);

        if (parentObject != null)
        {
            int childCount = parentObject.transform.childCount;
            return childCount;
        }
        else
        {
            Debug.LogWarning("Parent object not found: " + objectName);
            return -1;
        }
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
