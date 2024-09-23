using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LiquidVolumeFX;
using TMPro;
using UnityEngine;

public class MttAssaySceneScript : MonoBehaviour
{
    public enum GameStep
    {
        Start,
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
        VortexCellsAgain,
        PipetteToHemocytometer,
        PlaceHemocytomerUnderMicroscope,
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

    private enum TextLocation
    {
        Fridge,
        Heater,
        Hood,
        Microscope
    }
    private StartBtn startBtn;
    private InstructionScript instruction;
    private NotebookScript notebook;
    private GameStep currentStep;

    private VrTextPanel vrTextPanel;
    private TextMeshProUGUI hud_text;

    private bool trypsinAndMediumInIncubator = false;
    private bool trypsinAndMediumAreHeated = false;
    private bool TrypsinAndMediumInFumeHood = false;

    // Tracked Objects
    private GameObject CellCultureFlask;
    private LiquidTransfering CCFLiquidTransfer;
    private CellCultureFlaskShook CCFShakeScript;
    private VolumeChange CCFChange;
    private GameObject SingleConicalTube;
    private GameObject MultiConicalTube;
    private VolumeChange CTChange;
    private Centrifuge centrifuge;
    private LiquidTransfering CTLiquid;
    private LiquidTransfering CTMultiLiquid;
    private LiquidVolume LCTLayers;
    private  Vortex vortex;
    private LiquidTransfering mediumLiquid;
    private Hemocytometer hemocytometer;
    private GameObject singleChannelPipette;
    private GameObject Eppendorf;
    private LiquidVolume EppendophLV;
    private LiquidTransfering EppendorphLT;
    private Microscope microscope;

    void Start()
    {
        // Tracked Objects
        CellCultureFlask = GameObject.Find("CellCultureFlask");
        CCFLiquidTransfer = CellCultureFlask.GetComponent<LiquidTransfering>();

        CCFShakeScript = CellCultureFlask.GetComponent<CellCultureFlaskShook>();
        CCFChange = CellCultureFlask.GetComponent<VolumeChange>();

        SingleConicalTube = GameObject.Find("CylinderSingle");
        CTLiquid = SingleConicalTube.GetComponent<LiquidTransfering>();
        CTChange = SingleConicalTube.GetComponent<VolumeChange>();

        MultiConicalTube = GameObject.Find("CylinderLayered");
        LCTLayers = MultiConicalTube.GetComponent<LiquidVolume>();
        CTMultiLiquid = MultiConicalTube.GetComponent<LiquidTransfering>();
        MultiConicalTube.SetActive(false);

        centrifuge = FindObjectOfType<Centrifuge>();

        vortex = FindObjectOfType<Vortex>();

        mediumLiquid = GameObject.Find("Medium").GetComponentInChildren<LiquidTransfering>();

        singleChannelPipette = GameObject.Find("Single-Channel-Pippette");

        hemocytometer = FindObjectOfType<Hemocytometer>();

        Eppendorf = GameObject.Find("Eppendorf");
        EppendophLV = Eppendorf.GetComponentInChildren<LiquidVolume>();
        EppendorphLT = Eppendorf.GetComponentInChildren<LiquidTransfering>();

        microscope = GameObject.Find("Microscope").GetComponent<Microscope>();
        // Debug.Log(microscope);

        vrTextPanel = FindObjectOfType<VrTextPanel>();

        // Get the game text object
        hud_text = GameObject.Find("hud_text_tmp").GetComponent<TextMeshProUGUI>();

        
        if (hud_text == null)
        {
            Debug.LogError("Error no hud_text");
        }

        // Find the instructionObject GameObject and get the InstructionScript component
        GameObject instructionObject = GameObject.Find("Instructions");

        if (instructionObject != null)
        {
            instruction = instructionObject.GetComponent<InstructionScript>();
        }

        startBtn = FindAnyObjectByType<StartBtn>();

        // if (instruction != null)
        // {
        //     instruction.postBulletPoints("Procedure", "Locate the solvent cabinet", "-Hint: flammable, yellow");
        // }
        // else
        // {
        //     Debug.LogError("InstructionScript not found!");
        // }

        // GameObject notebookObject = GameObject.Find("Notebook");

        // if (notebookObject != null)
        // {
        //     notebook = notebookObject.GetComponent<NotebookScript>();
        // }

        // if (notebook != null)
        // {
        //     notebook.postText("Instrument Parameters");
        // }
        // else
        // {
        //     Debug.LogError("NotebookScript not found!");
        // }

        // Initialize the first game step
        currentStep = GameStep.Start;

        // hud_text.text = "Welcome to the MTT Assay Lab!\nRetrieve “Trypsin EDTA” from the -20\u00B0C freezer and “complete medium”\nfrom the 4\u00B0C fridge and place in the incubator to start.";
        hud_text.text = "Welcome to the MTT Assay Lab!\nClick START to begin";


    }

    void Update()
    {
        switch (currentStep)
        {
            case GameStep.Start:
                if (startBtn.start){
                    vrTextPanel.CardMoveTo((int)TextLocation.Fridge);
                    CompleteStep(GameStep.TrypsinAndMediumInHeater, "Place the Trypsin and Medium from the fridge into the heater");
                }
            break;
            case GameStep.TrypsinAndMediumInHeater:
                if (trypsinAndMediumInIncubator)
                {
                    // Proceed to the next step
                    CompleteStep(GameStep.WarmReagents, "Close the door and hit run to warm to 37\u00B0C.");
                }
                break;
            case GameStep.WarmReagents:
                if (trypsinAndMediumAreHeated){
                    vrTextPanel.CardMoveTo((int)TextLocation.Heater);
                    CompleteStep(GameStep.BringToCellCultureHood, "Bring the warmed reagent and medium to the “cell culture hood”");
                }
                break;

            case GameStep.BringToCellCultureHood:
                if (TrypsinAndMediumInFumeHood){
                    vrTextPanel.CardMoveTo((int)TextLocation.Hood);
                    CompleteStep(GameStep.RemoveSpentMedia, "Using the “stripette” remove the spent media from the\n“culture flask” containing cancer cells");
                }
                break;
            case GameStep.RemoveSpentMedia:
                if (CCFChange.IsClean()){    
                    CCFLiquidTransfer.amountContainer = 0.2f;
                    CCFLiquidTransfer.amountPipette = 0f;
                    CompleteStep(GameStep.AddTrypsin, "Using the stripette, add the Trypsin EDTA to the cell culture flask");
                }
            break;
            case GameStep.AddTrypsin:
                if (CCFChange.HasTrypsin()){        
                    CCFLiquidTransfer.amountContainer = 0.4f;
                    // CCFLiquidTransfer.amountPipette = 0f;
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
                    CCFLiquidTransfer.amountContainer = 0.3f;
                    CCFLiquidTransfer.amountPipette = 1f;
                }
            break;
            case GameStep.TransferToConicalTube:
                if (CTChange.IsClean()){
                    CompleteStep(GameStep.CentrifugeCells, "Centrafuge the tube");
                    centrifuge.CentrifugeActive = true;
                }
            break;
            case GameStep.CentrifugeCells:
                if (centrifuge.Centrafuged){
                    CompleteStep(GameStep.RemoveMediaFromPellet, "Remove media from tube an put back into medium bottle");
                    mediumLiquid.amountContainer = 0.9f;
                    mediumLiquid.amountPipette = 0f;
                }
            break;
            case GameStep.RemoveMediaFromPellet:
                if(LCTLayers.liquidLayers.Last().amount == 0f && mediumLiquid.GetContainerLevel() == 0.9f){
                    CompleteStep(GameStep.VortexCells, "Vortex the cell suspension");
                    vortex.VortexActive = true;
                }
            break;
            case GameStep.VortexCells:
                if (vortex.Vortexed){
                    CompleteStep(GameStep.AliquotCellSuspension, "Using the “single-channel pipette”, aliquot a portion of the cell suspension\ninto an “Eppendorf tube containing complete medium and trypan blue”");
                    vortex.VortexActive = false;
                    vortex.Vortexed = false;

                    CTMultiLiquid.SetPipette(singleChannelPipette);
                    CTMultiLiquid.amountContainer = 0.0f;
                    CTMultiLiquid.amountPipette = 0.8f;
                }
            break;
            case GameStep.AliquotCellSuspension:
                if (EppendophLV.level == .9f)
                {
                    CompleteStep(GameStep.VortexCellsAgain, "Vortex eppendorf tube");
                    vortex.tube = Eppendorf;
                    vortex.VortexActive = true;
                }
            break;
            case GameStep.VortexCellsAgain:
                if(vortex.Vortexed){
                    CompleteStep(GameStep.PipetteToHemocytometer, "Using the “single-channel pipette”, pipette a small amount onto the “hemocytometer");
                    hemocytometer.HemocytometerActive = true;
                    EppendorphLT.amountContainer = 0.7f;
                    EppendorphLT.amountPipette = 0.7f;
                }
            break;
            case GameStep.PipetteToHemocytometer:
                if (hemocytometer.Filled)
                {
                    CompleteStep(GameStep.PlaceHemocytomerUnderMicroscope, "Load hemocytometer into microscope");
                    microscope.microscopeActive = true;
                    hemocytometer.targetPipette.GetComponentInChildren<LiquidVolume>().level = 0f;
                    vrTextPanel.CardMoveTo((int)TextLocation.Microscope);
                }
            break;
            case GameStep.PlaceHemocytomerUnderMicroscope:
                if (microscope.IsSampleLoaded()){
                    CompleteStep(GameStep.ViewCellsUnderMicroscope, "Click microscope to view cells");
                }
            break;
            case GameStep.ViewCellsUnderMicroscope:
                if (microscope.IsViewing())
                {
                    
                }
            break;

        // Vortex the cell suspension and pipette a small amount onto the “hemocytometer”
        // Bring the hemocytometer to the “microscope” and view the cells through the eye lens
        // Count the number of live and dead cells in each corner and record in your notebook


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

        hud_text.text = instructionMessage;
    }
}
