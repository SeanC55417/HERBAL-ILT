using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MttAssaySceneScript : MonoBehaviour
{
    public enum GameStep
    {
        isTrue,
        TrypsinAndMediumInHeater,
        WarmReagents,
        BringToCellCultureHood,
        RemoveSpentMedia,
        AddTrypsin,
        AddMedium,
        ShakeFlask,
        TransferToConicalTube,
        CentrifugeCells,

        KnowledgeCheck1,

        RemoveMediaFromPellet,
        VortexCells,
        AliquotCellSuspension,
        ViewCellsUnderMicroscope,
        CountLiveAndDeadCells,

        KnowledgeCheck2,

        Retrieve96WellPlatesAndExtracts,
        LoadIntoPlatePrepper,
        IncubateOvernight,
        
        KnowledgeCheck3,

        AddXttReagent,
        IncubateAfterXttAddition,
        ReadPlateAtSpectrophotometer,
        
        KnowledgeCheck4,
        
        Done
    }

    private InstructionScript instruction;
    private NotebookScript notebook;
    private GameStep currentStep;

    public TextMeshProUGUI hud_text;

    private bool trypsinAndMediumInIncubator = false;
    private bool trypsinAndMediumAreHeated = false;
    private bool TrypsinAndMediumInFumeHood = false;

    // Tracked Objects
    private GameObject CellCultureFlask;
    private BottleCapInteraction CCFScipt;
    private CellCultureFlaskShook CCFShakeScript;
    private VolumeChange CCFChange;
    private GameObject ConicalTube;
    private VolumeChange CTChange;
    private GameObject Vortex;
    private Vortex vortex;

    void Start()
    {

        // Tracked Objects
        CellCultureFlask = GameObject.Find("CellCultureFlask");
        CCFScipt = CellCultureFlask.GetComponent<BottleCapInteraction>();
        CCFShakeScript = CellCultureFlask.GetComponent<CellCultureFlaskShook>();
        CCFChange = CellCultureFlask.GetComponent<VolumeChange>();

        ConicalTube = GameObject.Find("ConicalTube");
        CTChange = ConicalTube.GetComponent<VolumeChange>();

        Vortex = GameObject.Find("Vortex");         // This code is so evil ;)
        vortex = Vortex.GetComponent<Vortex>();

        // Find the instructionObject GameObject and get the InstructionScript component
        GameObject instructionObject = GameObject.Find("Instructions");

        if (instructionObject != null)
        {
            instruction = instructionObject.GetComponent<InstructionScript>();
        }

        if (instruction != null)
        {
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

        if (notebook != null)
        {
            notebook.postText("Instrument Parameters");
        }
        else
        {
            Debug.LogError("NotebookScript not found!");
        }

        // Initialize the first game step
        currentStep = GameStep.TrypsinAndMediumInHeater;

        // hud_text.text = "Welcome to the MTT Assay Lab!\nRetrieve “Trypsin EDTA” from the -20\u00B0C freezer and “complete medium”\nfrom the 4\u00B0C fridge and place in the incubator to start.";
    }

    void Update()
    {
        switch (currentStep)
        {
            case GameStep.TrypsinAndMediumInHeater:
                if (trypsinAndMediumInIncubator)
                {
                    // Proceed to the next step
                    CompleteStep(GameStep.WarmReagents, "Close the door and hit run to warm to 37\u00B0C.");
                }
                break;
            case GameStep.WarmReagents:
                if (trypsinAndMediumAreHeated){
                    CompleteStep(GameStep.BringToCellCultureHood, "Bring the warmed reagent and medium to the “cell culture hood”");
                }
                break;

            case GameStep.BringToCellCultureHood:
                if (TrypsinAndMediumInFumeHood){
                    CompleteStep(GameStep.RemoveSpentMedia, "Using the “stripette” remove the spent media from the\n“culture flask” containing cancer cells");
                }
                break;
            case GameStep.RemoveSpentMedia:
                if (CCFChange.IsClean()){    
                    CCFScipt.fillBottle = 0.2f;
                    CCFScipt.type = BottleCapInteraction.Exchange.Fill;        
                    CompleteStep(GameStep.AddTrypsin, "Using the stripette, add the Trypsin EDTA to the cell culture flask");
                }
            break;
            // case GameStep.isTrue:

            //     CompleteStep(GameStep.AddTrypsin, "Using the stripette, add the Trypsin EDTA to the cell culture flask");
            // break;
            case GameStep.AddTrypsin:
                if (CCFChange.HasTrypsin()){        
                    CCFScipt.fillBottle = 0.4f;
                    CompleteStep(GameStep.AddMedium, "Using the stripette, add complete medium to the flask");
                }
            break;
            case GameStep.AddMedium:
                if (CCFChange.HasMedium()){        
                    CompleteStep(GameStep.ShakeFlask, "Shake flask to mix the dislodged cells");
                }
            break;
            case GameStep.ShakeFlask:
                CCFShakeScript.isShakeDetectionActive = true;
                if(CCFShakeScript.IsShaken()){
                    CompleteStep(GameStep.TransferToConicalTube, "Using the stripette, transfer the cell suspension to a “conical tube”");
                    CCFScipt.fillBottle = 0.3f;
                    CCFScipt.fillPipette = 1f;
                    CCFScipt.type = BottleCapInteraction.Exchange.TransferFrom;
                }
            break;
            case GameStep.TransferToConicalTube:
                if (CTChange.IsClean()){
                    CompleteStep(GameStep.VortexCells, "Vortex the cell suspension and pipette a small amount onto the “hemocytometer”");
                }
            break;
            case GameStep.VortexCells:
                if (vortex.vortexed){
                    CompleteStep(GameStep.AliquotCellSuspension,"Using the “single-channel pipette”, aliquot a portion of the cell suspension\ninto an “Eppendorf tube containing complete medium and trypan blue”");
                }
            break;
        }   
    }

    public void SetTrypsinAndMediumInIncubator(bool inIncubator)
    {
        trypsinAndMediumInIncubator = inIncubator;
    }

    public void SetTrypsinAndMediumHeated()
    {
        trypsinAndMediumAreHeated = trypsinAndMediumInIncubator;
    }

    public void SetTrypsinAndMediumInFumeHood(bool inFumeHood)
    {
        TrypsinAndMediumInFumeHood = inFumeHood;
    }

    void CompleteStep(GameStep nextStep, string instructionMessage)
    {
        Debug.Log($"Step {currentStep} completed. Proceed to: {nextStep}");
        currentStep = nextStep;

        if (instruction != null)
        {
            hud_text.text = instructionMessage;
        }
    }
}
