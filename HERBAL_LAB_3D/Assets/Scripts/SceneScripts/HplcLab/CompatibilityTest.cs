using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;


class CompatibilityTest : MonoBehaviour
{
    string[][] dataframe;
    public TextMeshProUGUI SelectedColumn;
    public TextMeshProUGUI CompatibilityOutput;
    public TextMeshProUGUI SelectedSolvent;
    string column;
    string solvent;

    public AudioClip compatibleSFX;
    public AudioClip semicompatibleSFX;
    public AudioClip noncompatibleSFX;
    public AudioClip nodataSFX;
    public AudioClip invalidSFX;

    public AudioSource audioSource;

    public void ConvertToString()
    {
        column = SelectedColumn.text;
        solvent = SelectedSolvent.text;
    }

    public void GetDatabase()
    {
        var basePath = Path.Combine(Application.streamingAssetsPath, "Solvent-Media Compatibility.csv");
        string[] lines = File.ReadAllLines(basePath);
        dataframe = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            dataframe[i] = lines[i].Split(',');
        }
        /*String fileName = "Assets/Resources/Solvent-Media Compatibility.csv";
        var filePath = fileName;
        dataframe = fileReadIn.text.Select(x => x.Split(',')).ToArray();
        if (dataframe != null)
        {
            Debug.Log(dataframe);
        }
        string[] lines = File.ReadAllLines(fileName);
        dataframe = new string[lines.Length][];
        for (int i = 0; i < lines.Length; i++)
        {
            dataframe[i] = lines[i].Split(',');
        }*/
    }

    public void FindCompatibility()
    {
        bool columnCheck = false;
        bool solventCheck = false;
        int position = -1;
        string compatibility = "-2";
        string substance;
        int mediaPosition = -2;
        int y = 0;

        for (int i = 1; i < dataframe[1].GetLength(0); i++)
        {
            substance = dataframe[0][i];
            if (column == substance)
            {
                columnCheck = true;
            }
        }

        for (int i = 1; i < dataframe.GetLength(0) - 7; i++)
        {
            substance = dataframe[i][0];
            if (solvent == substance)
            {
                solventCheck = true;
            }
        }

        if (columnCheck == true && solventCheck == true)
        {

            //index array
            for (int i = 1; i < dataframe.GetLength(0) - 7; i++)
            {
                substance = dataframe[i][0];
                if (substance == solvent)
                {
                    y = i;

                }

            }
            mediaPosition = -1;
            if (column == "Cellulose Mixed Esters")
            {
                position = 0;
            }
            else if (column == "Cellulose Acetate")
            {
                position = 1;
            }
            else if (column == "Regenerated Cellulose")
            {
                position = 2;
            }
            else if (column == "Polyamide")
            {
                position = 3;
            }
            else if (column == "Teflon")
            {
                position = 4;
            }
            else if (column == "Polyvinylidene difluoride")
            {
                position = 5;
            }
            else if (column == "Polyethersulfone")
            {
                position = 6;
            }
            else if (column == "Polyester")
            {
                position = 7;
            }
            else if (column == "Glass Fiber")
            {
                position = 8;
            }
            else if (column == "Polypropylene")
            {
                position = 9;
            }
            else if (column == "Silica")
            {
                position = 10;
            }
            else if (column == "C18")
            {
                position = 11;
            }
            mediaPosition = position;

            //index array
            //int compatible = Int32.Parse(compatibility);
        }
        if (!solventCheck && !columnCheck)
        {
            CompatibilityOutput.text = "column/solvent is invalid";
            audioSource.clip = invalidSFX;
        }
        else if (!solventCheck)
        {
            CompatibilityOutput.text = "the entered solvent was invalid";
            audioSource.clip = invalidSFX;
        }
        else if (!columnCheck)
        {
            CompatibilityOutput.text = "The entered column was invalid";
            audioSource.clip = invalidSFX;
        }
        else
        {
            compatibility = dataframe[y][mediaPosition + 1];
            //print compatibility
            if (compatibility == "-1")
            {
                CompatibilityOutput.text = "Non-Resistant";
                audioSource.clip = noncompatibleSFX;
            }
            else if (compatibility == "0")
            {
                CompatibilityOutput.text = "Semi-Resistant";
            }
            else if (compatibility == "1")
            {
                CompatibilityOutput.text = "Resistant";
                audioSource.clip = compatibleSFX;
            }
            else
            {
                CompatibilityOutput.text = "missing data";
                audioSource.clip = nodataSFX;
            }
        }
    }

    public void audioOnClick()
    {
        audioSource.Play();
    }
}
