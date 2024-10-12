using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;

public class InfoCard : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject infoCard;

    public int sceneNumber = 0;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI stats;
    public Image moduleImg;

    // This function shows the card with the information for the specified sceneId
    public void ShowCard(int sceneId)
    {
        infoCard.SetActive(true);
    
        Dictionary<string, string> dataForID = new Dictionary<string, string>();

        // Load the CSV file from the Resources folder
        TextAsset csvFile = Resources.Load<TextAsset>("Data/ModuleInfo");
        if (csvFile == null)
        {
            Debug.LogError("Failed to load CSV file from Resources.");
            return;
        }

        using (var reader = new System.IO.StringReader(csvFile.text))
        {
            // Read and parse the first line to get scene IDs
            string headerLine = reader.ReadLine();
            string[] headers = ParseCSVLine(headerLine);
            int targetIndex = -1;
            string targetID = sceneId.ToString();

            for (int i = 1; i < headers.Length; i++)
            {
                headers[i] = headers[i].Trim();  // Trim spaces from headers to avoid mismatches
                if (headers[i] == targetID)
                {
                    targetIndex = i;
                    break;
                }
            }

            if (targetIndex == -1)
            {
                Debug.LogError("Target ID not found in the CSV file.");
                return;
            }

            while (reader.Peek() != -1)
            {
                var line = reader.ReadLine();
                var values = ParseCSVLine(line);
                string attribute = values[0].Trim(); // First cell is the attribute name

                if (values.Length > targetIndex) // Ensure there is a value for the target ID
                {
                    dataForID[attribute] = values[targetIndex].Trim();  // Trim spaces and store data
                }
            }
        }

        // Update the TextMeshPro UI elements with data
        title.text = dataForID.TryGetValue("Title", out string titleText) ? titleText : "Title Not Found";
        description.text = dataForID.TryGetValue("Description", out string descText) ? descText : "Description Not Found";
        
        // Assuming 'stats' field should include both 'Assessment' and 'Time'
        dataForID.TryGetValue("Assessment", out string assessmentText);
        dataForID.TryGetValue("Time", out string timeText);
        stats.text = "Assessment: " + assessmentText + "\nTime: " + timeText + " Mins";
        
        if (dataForID.TryGetValue("ImagePath", out string imagePath))
        {
            Sprite sprite = Resources.Load<Sprite>(imagePath);
            if (sprite != null)
            {
                moduleImg.sprite = sprite;
                Debug.Log("Setting Sprite is: " + imagePath);
            }
            else
            {
                Debug.LogError("Failed to load the image: " + imagePath);
            }
        }
        else
        {
            Debug.LogError("Image path not found in data.");
        }
        gameManager.nextScene = dataForID.TryGetValue("SceneName", out string sceneName) ? sceneName : "TutorialRoom";
    }

    // This function hides the card
    public void HideCard()
    {
        infoCard.SetActive(false);
    }

    // Function to parse a CSV line considering quoted fields with commas
    private string[] ParseCSVLine(string line)
    {
        var pattern = new Regex(
            @"    # Match one value in valid CSV string.
            \s*    # Ignore leading whitespace.
            (?:    # Group for value alternatives.
              ""    # Either a double quoted value.
              (?<val>    # Group to capture value.
                [^""]*(""""[^""]*)*    # Zero or more non-quotes, allowing doubled "" as escape.
              )""
            |  # Or...
              (?<val>[^,]*)    # Non-quoted value.
            ) 
            \s*    # Ignore trailing whitespace.
            (?:,|$)    # Field ends on comma or EOS.",
            RegexOptions.IgnorePatternWhitespace | RegexOptions.ExplicitCapture);

        List<string> values = new List<string>();
        foreach (Match m in pattern.Matches(line))
        {
            string value = m.Groups["val"].Value;
            value = value.Replace("\"\"", "\"");  // Replace double quotes with single quotes
            values.Add(value);
        }

        return values.ToArray();
    }
}
