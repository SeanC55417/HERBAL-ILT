using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI playerHudText;   // Text
    private int currentStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTutorialHud();
    }

    // void Update()
    // {
        
    // }

    public void AdvanceTutorial()
    {
        currentStep++; // Increment the tutorial step
        UpdateTutorialHud(); // Update the HUD for the new step
    }

    private void UpdateTutorialHud()
    {
        switch (currentStep)
        {
            case 0:
                playerHudText.text = "Welcome the 3D Build, Move with WASD or arrow keys to continue";
                // Activate and wait for finish of movement script
                break;
            case 1:
                playerHudText.text = "Now move to the wardrobe and equip your personal protective equipment";
                // Activate and wait for finish of ppe script
                break;
            default:
                playerHudText.text = "Congrats you have completed the tutorial";
                break;
        }
    }
}
