using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI playerHudText;   // Text
    public Confetti confetti;

    public enum GameStep{
        walk,
        equipPpe,
        moveObject,
        openNotebook,
        done
    }

    private GameStep currentStep = GameStep.walk;

    // Start is called before the first frame update
    void Start()
    {
        playerHudText.text = "Welcome the 3D Build, Move with WASD or arrow keys to continue";
    }

    void Update()
    {
        switch (currentStep)
        {
            case GameStep.walk:
                if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
                    CompleteStep(GameStep.equipPpe, "Toggling items is done with a \'mouse click\'\n\nMove to the wardrobe and put on your personal protective equipment");
                }
                break;
            case GameStep.equipPpe:
                GameObject ppeObj = GameObject.Find("PPE Objects");
                if (ppeObj != null){
                    int childCount = ppeObj.transform.childCount;
                    if (childCount == 0){
                        CompleteStep(GameStep.moveObject, "Pick up the Flask with the \'E\' key and move it from pillar 1 to pillar 2\n\nIf you ever need to reset the objects position in your hand press the \'Space Bar\'");
                    }
                }
                break;
            case GameStep.moveObject:
                if (IsRbObjectSet("Flask")){
                    confetti.StartConfetti("", 10f);
                    CompleteStep(GameStep.openNotebook, "Congrats!\nOpen the menu panel with \'M\', click the menu button and exit or restart module");
                }
                break;
            case GameStep.openNotebook:
                if (Input.GetKeyDown(KeyCode.M)){
                    CompleteStep(GameStep.done, "");
                }
                break;
            case GameStep.done:
                break;
        }
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
            Debug.Log("ERROR 2");
        }
        Debug.Log("ERROR 1");
        return false;
    }

    void CompleteStep(GameStep nextStep, string instructionMessage)
    {
        Debug.Log($"Step {currentStep} completed. Proceed to: {nextStep}");
        currentStep = nextStep;
        
        playerHudText.text = instructionMessage;
    }
}
