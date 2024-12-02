using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HplcSceneScript : MonoBehaviour
{
    public enum GameStep
    {
        Start,
        OpenFireCabinet,
        BringWaterToHplc,
        BringMethanolToHplc,
        OpenHplcOven,
        OpenDrawer,
        PlaceColumnInHplc,
        SelectProperFlowDirection,
        CloseHplcOven,
        
        KnowledgeCheck1Q1,
        KnowledgeCheck1Q2,
        
        OpenHplcTraySlot,
        SetSampleTray,
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

    private enum TextLocation
    {
        FireCabinetAndComputer,
        HPLC,
        SampleTable
    }
    
    private StartBtn startBtn;
    public QuestionScript questionScript;
    public NotebookScript notebook;
    private GameStep currentStep;
    private GameObject computerScreen = null;

    public TextMeshProUGUI hud_text;
    public Confetti confetti;
    private VrTextPanel vrTextPanel;
    private float waitTimeAfterCorrectAnswer = 0.5f;
   

    void Start()
    {

        // Ensure the script is found before trying to call its methods
        if (notebook != null)
        {
            notebook.postText("Instrument Parameters");
        }
        else
        {
            Debug.LogError("NotebookScript not found!");
        }

        currentStep = GameStep.Start;
        hud_text.text = "Welcome to the HPLC Lab!";
        vrTextPanel = FindObjectOfType<VrTextPanel>();
        startBtn = FindAnyObjectByType<StartBtn>();

    }

    void Update()
    {
        switch (currentStep)
        {
            case GameStep.Start:
                if (startBtn.start){
                    vrTextPanel.CardMoveTo((int)TextLocation.FireCabinetAndComputer);
                    CompleteStep(GameStep.OpenFireCabinet, "Open the flammable solvent cabinet to obtain your mobile");
                }
            break;
            case GameStep.OpenFireCabinet:
                if (IsFireCabinetOpened())
                {
                    CompleteStep(GameStep.BringWaterToHplc, "Bring the water to the HPLC");
                }
                break;
                
            case GameStep.BringWaterToHplc:
                if (IsRbObjectSet("Solvent Bottle Water"))
                {
                    CompleteStep(GameStep.BringMethanolToHplc, "Bring the methanol to the HPLC");
                    notebook.postText("\nPolar Sol. A: Water");
                }
                break;
            case GameStep.BringMethanolToHplc:
                if (IsRbObjectSet("Solvent Bottle Methanol"))
                {
                    vrTextPanel.CardMoveTo((int)TextLocation.HPLC);
                    CompleteStep(GameStep.OpenHplcOven, "Open the HPLC column compartment");
                    notebook.postText("\nOrganic Sol. B: Methanol");
                }
                break;
            case GameStep.OpenHplcOven:
                if (IsHplcOvenOpen())
                {
                    CompleteStep(GameStep.OpenDrawer, "Open the column drawer underneath the HPLC");
                }
                break;
            case GameStep.OpenDrawer:
                if (IsColumnDrawerOpen())
                {
                    CompleteStep(GameStep.PlaceColumnInHplc, "Place the column inside the column compartment");
                }
                break;
            case GameStep.PlaceColumnInHplc:
                if (ChildrenInParent("Column Latches") == 1)
                {
                    CompleteStep(GameStep.SelectProperFlowDirection, "Select the proper flow direction");
                }
                break;
            case GameStep.SelectProperFlowDirection:
                if (flowDirectionSet())
                {
                    CompleteStep(GameStep.CloseHplcOven, "Close the column compartment");
                }
            break;
            case GameStep.CloseHplcOven:
                if (!IsHplcOvenOpen())
                {
                    // CompleteStep(GameStep.OpenHplcTraySlot, "Open HPLC vial rack");
                    CompleteStep(GameStep.KnowledgeCheck1Q1, "");
                    questionScript.PostQuestion("Question1");
                }
                break;
            case GameStep.KnowledgeCheck1Q1:
                if (questionScript.getNumAnswered() > 0){
                    Wait(waitTimeAfterCorrectAnswer);
                    CompleteStep(GameStep.KnowledgeCheck1Q2, "Great Job!");
                    questionScript.PostQuestion("Question2");
                }
                break;
            case GameStep.KnowledgeCheck1Q2:
                if (questionScript.getNumAnswered() > 1){
                    Wait(waitTimeAfterCorrectAnswer);
                    CompleteStep(GameStep.OpenHplcTraySlot, "Open HPLC vial rack");
                    questionScript.deactivateQuestionPanel();
                }
                break;
            case GameStep.OpenHplcTraySlot:
                if(IsTraySlotOpen())
                {
                    vrTextPanel.CardMoveTo((int)TextLocation.SampleTable);
                    CompleteStep(GameStep.SetSampleTray, "Place sample vials into sample tray (hint: click on the samples)");
                }
                break;
            case GameStep.SetSampleTray:
                if (ChildrenInParent("Sample Bottles") == 0)
                {
                    vrTextPanel.CardMoveTo((int)TextLocation.HPLC);
                    CompleteStep(GameStep.PlaceTrayInHplc, "Place the sample tray in the vial rack");
                }
                break;
            case GameStep.PlaceTrayInHplc:
                if(ChildrenInParent("Tray Destination") == 1)
                {
                    CompleteStep(GameStep.CloseHplcTraySlot, "Close the HPLC vial rack");
                }
                break;
            case GameStep.CloseHplcTraySlot:
                if (!IsTraySlotOpen()) 
                {
                    vrTextPanel.CardMoveTo((int)TextLocation.FireCabinetAndComputer);
                    CompleteStep(GameStep.EnterMethod, "Enter your instrument starting parameters on the computer by selecting the white boxes");
                    FlashHplcButtons();
                }
                break;
            case GameStep.EnterMethod:
                if (ChildrenInParent("Set Up Screen") == 3){
                    CompleteStep(GameStep.KnowledgeCheck2Q1, "");
                    questionScript.PostQuestion("Question3");
                }
                break;
            case GameStep.KnowledgeCheck2Q1:
                if (questionScript.getNumAnswered() > 2){
                    Wait(waitTimeAfterCorrectAnswer);
                    CompleteStep(GameStep.KnowledgeCheck2Q2, "");
                    questionScript.PostQuestion("Question4");
                }
                break;
            case GameStep.KnowledgeCheck2Q2:
                if (questionScript.getNumAnswered() > 3){
                    Wait(waitTimeAfterCorrectAnswer);
                    questionScript.deactivateQuestionPanel();
                    CompleteStep(GameStep.SelectPump, "Awesome!\nRefer back to the HPLC computer screen and press pump “ON” to begin equilibrating your column");
                    GameObject pumpButton = GameObject.Find("Pump Button");
                    Collider collider = pumpButton.GetComponent<Collider>();
                    collider.enabled = true;
                    ButtonIsFlashing("Pump Button", true);
                }
                break;
            case GameStep.SelectPump:
                GameObject pumpRunningCanvas;
                pumpRunningCanvas = GameObject.Find("Pump Running Canvas");

                if (pumpRunningCanvas != null){
                    CompleteStep(GameStep.SelectQuickBatch, "Select \"Quick Batch\" on the computer");
                    ButtonIsFlashing("Batch Button", true);
                }
                break;
            case GameStep.SelectQuickBatch:
                computerScreen = GameObject.Find("Screen Canvas");
                if (computerScreen != null)
                {
                    GameObject batchScreen = GameObject.Find("PC Batch Mode Screen");
                    if (batchScreen != null)
                    {
                        CompleteStep(GameStep.AssignSamples, "Assign samples (Optional for now) then click Start");
                                                ButtonIsFlashing("Batch Button", false);
                    }
                }
                break;
            case GameStep.AssignSamples:
                computerScreen = GameObject.Find("Screen Canvas");
                if (computerScreen != null)
                {
                    GameObject runningScreen = GameObject.Find("Running Canvas");
                    if (runningScreen != null)
                    {
                        CompleteStep(GameStep.RunHplc, "Run HPLC");
                    }
                }
                break;
            case GameStep.RunHplc:
                GameObject allGraphsScreen = GameObject.Find("PC Screen Graphs");
                if (allGraphsScreen != null)
                {
                    CompleteStep(GameStep.KnowledgeCheck3Q1, "Press M to bring up the notebook to do KnowledgeCheck3");
                    questionScript.PostQuestion("Question5");
                }
                break;
            case GameStep.KnowledgeCheck3Q1:
                if (questionScript.getNumAnswered() > 4){
                    CompleteStep(GameStep.KnowledgeCheck3Q2, "");
                    questionScript.PostQuestion("Question6");
                }
                break;
            case GameStep.KnowledgeCheck3Q2:
                if (questionScript.getNumAnswered() > 5){
                    CompleteStep(GameStep.KnowledgeCheck4Q1, "Refer to the chromatograms you generated during the experiment to complete the next set of questions");
                    questionScript.PostQuestion("Question7");
                }
                break;
            case GameStep.KnowledgeCheck4Q1:
                if (questionScript.getNumAnswered() > 6){
                    CompleteStep(GameStep.KnowledgeCheck4Q2, "Congratulations you finished the module! Use the notebook to return to the home screen");
                    questionScript.PostQuestion("Question8");
                    Debug.Log("running 1");
                }
                break;
            case GameStep.KnowledgeCheck4Q2:
                if (questionScript.getNumAnswered() > 8){
                    CompleteStep(GameStep.Done, "Congratulations you finished the module! Use the notebook to return to the home screen");
                    questionScript.deactivateQuestionPanel();
                }
                break;
            case GameStep.Done:
                Debug.Log("done 1");
                confetti.StartConfetti("Congratulations you finished the module! Use the notebook to return to the home screen", 10);
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

    bool flowDirectionSet()
    {
        GameObject FlowDirectionDown = GameObject.Find("Flow Down Color");

        if (FlowDirectionDown != null)
        {
            Image imageComponent = FlowDirectionDown.GetComponent<Image>();

            if (imageComponent != null)
            {
                Color desiredColor;
                if (ColorUtility.TryParseHtmlString("#05FF00", out desiredColor))
                {
                    // Compare the color of the image component to the desired color
                    if (imageComponent.color == desiredColor)
                    {
                        Debug.Log("green");
                        return true;
                    }
                }
            }
        }

        return false;
    }


    bool IsHplcOvenOpen()
    {
        GameObject hplcOvenDoor = GameObject.Find("Column Door");
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
        GameObject columnDrawer = GameObject.Find("Left Vail Rack");
        if (columnDrawer != null)
        {
            float xPosition = columnDrawer.transform.localPosition.x;

            if (xPosition > 0.6)
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
    public void ButtonIsFlashing(string button, bool flash)
    {
        GameObject batchBtn = GameObject.Find(button);
        if (batchBtn != null){
            ButtonFlash buttonFlash = batchBtn.GetComponent<ButtonFlash>();
            if (buttonFlash != null)
            {
                if (flash)
                {
                    buttonFlash.StartFlashing();
                }
                else 
                {
                    buttonFlash.StopFlashing();
                }
            }
        }
    }

    public void FlashHplcButtons()
    {
        GameObject parentObject = GameObject.Find("Set Up Screen");
        if (parentObject != null)
        {
            Transform[] hplcParameterBtns = parentObject.GetComponentsInChildren<Transform>();
            foreach (Transform btnTransform in hplcParameterBtns)
            {
                GameObject btn = btnTransform.gameObject;
                if (btn.name != "Batch Button" && btn.name != "Pump Button")
                {
                    ButtonFlash buttonFlash = btn.GetComponent<ButtonFlash>();
                    if (buttonFlash != null)
                    {
                        buttonFlash.StartFlashing();
                    }
                }
            }
        }
        else {
            Debug.Log("Could not find Hplc Screen");
        }
    }

    void CompleteStep(GameStep nextStep, string instructionMessage)
    {
        Debug.Log($"Step {currentStep} completed. Proceed to: {nextStep}");
        currentStep = nextStep;

        hud_text.text = instructionMessage;
    }

    private IEnumerator Wait(float secondsWaiting)
    {
        yield return new WaitForSeconds(secondsWaiting);
    }

}