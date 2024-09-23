using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MiscibilityTest : MonoBehaviour
{
    string[][] dataframe;
    public TextMeshProUGUI SelectedSolvent1;
    public TextMeshProUGUI SelectedSolvent2;
    public TextMeshProUGUI output;
    public static string solvent1;
    public static string solvent2;

    public AudioClip miscibleSFX;
    public AudioClip nonmiscibleSFX;
    public AudioClip nodataSFX;

    public Button miscibilityButton;
    public AudioSource audioSource;

    public void ConvertToString()
    {
        solvent1 = SelectedSolvent1.text;
        solvent2 = SelectedSolvent2.text;
    }

    public void GetDatabase()
    {
        var basePath = Path.Combine(Application.streamingAssetsPath, "Solvent-Miscibility as.csv");
        string[] lines = File.ReadAllLines(basePath);
        dataframe = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            dataframe[i] = lines[i].Split(',');
        }

        /*String fileName = "Assets/Resources/Solvent-Miscibility as.csv";
        var filePath = fileName;
        string[] lines = File.ReadAllLines(filePath);
        dataframe = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            dataframe[i] = lines[i].Split(',');
        }*/
    }

    public void FindMiscibility()
    {
        bool substanceCheck1 = false;
        bool substanceCheck2 = false;
        string dst = "-1";

        string substance;

        for (int i = 0; i < dataframe.GetLength(0); i++)
        {
            substance = dataframe[i][0];
            if (solvent1 == substance)
            {
                substanceCheck1 = true;
            }
        }

        for (int i = 0; i < dataframe.GetLength(0); i++)
        {
            substance = dataframe[i][0];
            if (solvent2 == substance)
            {
                substanceCheck2 = true;
            }
        }
        if (substanceCheck1 == true && substanceCheck2 == true)
        {
            string miscibility = "";
            int y = 0;

            //index array
            for (int i = 0; i < dataframe.GetLength(0); i++)
            {
                substance = dataframe[i][0];
                if (substance == solvent1)
                {
                    y = i;
                }
            }

            //index array
            for (int i = 0; i < dataframe.GetLength(0); i++)
            {
                substance = dataframe[i][0];
                if (substance == solvent2)
                {
                    miscibility = dataframe[i][y];
                }
            }

            dst = miscibility;

            if (dst == "0")
            {
                output.text = "Non-Miscible";
                audioSource.clip = nonmiscibleSFX;
            }
            if (dst == "1")
            {
                output.text = "Miscible";
                audioSource.clip = miscibleSFX;
            }
            if (dst == "")
            {
                output.text = "Missing data";
                audioSource.clip = nodataSFX;
            }
        }
        else
        {
            output.text = "One or Both of the entered solvents were invalid";
        }
    }

    public void audioOnClick()
    {
        audioSource.Play();
    }
}