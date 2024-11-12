using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using TMPro;

public class QuestionScript : MonoBehaviour
{
    private Dictionary<string, string[]> knowledgeCheckQuestions = new Dictionary<string, string[]>();
    private Dictionary<string, int> QuestionHeaderIndexes = new Dictionary<string, int>();
    public GameObject answerPrefab;
    private TextMeshProUGUI QuestionText;
    public TextMeshProUGUI hud_text;
    
    // Add these fields to track progress and objects
    private int questionsAnswered = 0;
    private string currentQuestion; // Reference for current question

    public void getQuestions()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("Data/Questions");

        using (StringReader sr = new StringReader(csvFile.text))
        {
            List<string> lines = new List<string>();
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }

            string[] firstRow = lines[0].Split(',');

            for (int i = 0; i < firstRow.Length; i++)
            {
                string word = firstRow[i];
                if (word == "Questions")
                {
                    QuestionHeaderIndexes["Questions"] = i - 1;
                }
                else if (word == "Hint")
                {
                    QuestionHeaderIndexes["Hint"] = i - 1;
                }
                else if (word == "Correct Answer(s)")
                {
                    QuestionHeaderIndexes["Correct Answer(s)"] = i - 1;
                }
            }

            for (int i = 0; i < lines.Count; i++)
            {
                string[] words = lines[i].Split(',');

                if (words.Length > 1)
                {
                    string key = words[0];
                    string[] values = new string[words.Length - 1];
                    Array.Copy(words, 1, values, 0, words.Length - 1);

                    knowledgeCheckQuestions[key] = values;
                }
            }
        }
    }

    public void PostQuestion(string questionKey)
    {
        hud_text.gameObject.SetActive(false);
        gameObject.SetActive(true);
        getQuestions();
        // Checks if the question key is loaded into the dictionary
        if (knowledgeCheckQuestions.ContainsKey(questionKey))
        {
            // Finds and sets the question text by looking into the game object the script is attached to 
            QuestionText = gameObject.transform.Find("QuestionText").GetComponent<TextMeshProUGUI>();
            // Indexes the first element in the dictionary key and sets the question text
            QuestionText.text = knowledgeCheckQuestions[questionKey][0];

            // Gets the question text rect transform
            RectTransform QuestionRectTransform = QuestionText.GetComponent<RectTransform>();
            QuestionRectTransform.sizeDelta = new Vector2(QuestionRectTransform.sizeDelta.x, QuestionText.preferredHeight);

            Transform AnswersContainer = gameObject.transform.Find("Answers");
            GridLayoutGroup AnswersGridLayout = AnswersContainer.GetComponent<GridLayoutGroup>();

            if (AnswersContainer.childCount > 0)
            {
                for (int i = 0; i < AnswersContainer.childCount; i++)
                {
                    Destroy(AnswersContainer.GetChild(i).gameObject);
                }
            }

            float cellWidth = 120;
            float cellHeight = 50;

            
            int answerCount = 0;
            for (int i = 1; i < knowledgeCheckQuestions[questionKey].Length - 2; i++)
            {   
                if (knowledgeCheckQuestions[questionKey][i] != "" && i < QuestionHeaderIndexes["Hint"])
                {
                    answerCount++;
                }
            }

            if (answerCount < 4)
            {
                Debug.Log("change answer box");
                cellWidth = 250;
                cellHeight = 30;
            }

            // Loops through each answer given
            for (int i = 1; i < answerCount + 1; i++)
            {
                // Debug.Log("Answer: " + knowledgeCheckQuestions[questionKey][i]);
                GameObject currentAnswerOption = Instantiate(answerPrefab, AnswersContainer.transform);
                Button button = currentAnswerOption.GetComponent<Button>();
                TextMeshProUGUI childTextMeshPro = currentAnswerOption.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                RectTransform childRectTransform = currentAnswerOption.GetComponent<RectTransform>();
                BoxCollider boxCollider = currentAnswerOption.GetComponent<BoxCollider>();
                button.onClick.AddListener(() => checkAnswer(button, childTextMeshPro));
                currentAnswerOption.name = "Answer " + i;

                childTextMeshPro.text = knowledgeCheckQuestions[questionKey][i];
                if (childTextMeshPro.preferredHeight > cellHeight || answerCount < 4)
                {
                    AnswersGridLayout.cellSize = new Vector2(cellWidth, cellHeight);
                    childRectTransform.sizeDelta = new Vector2(childRectTransform.sizeDelta.x, childTextMeshPro.preferredHeight + 20);
                    boxCollider.size = new Vector2(childRectTransform.sizeDelta.x, childTextMeshPro.preferredHeight + 20);
                    // currentAnswerOption. = new Vector2(childRectTransform.sizeDelta.x, childRectTransform.sizeDelta.y);
                }
                childRectTransform.sizeDelta = new Vector2(cellWidth, cellHeight);
                boxCollider.size = new Vector2(cellWidth, cellHeight);
                // currentQuestionHeight += childRectTransform.sizeDelta.y + 5;

            }
            currentQuestion = questionKey;
        }
        else
        {
            Debug.LogError("Can't find question key: " + questionKey + " in CSV");
        }
    }

    public void checkAnswer(Button button, TextMeshProUGUI buttonText)
    {
        // Get the Image component attached to the button
        Image buttonImage = button.GetComponent<Image>();
        ColorBlock colors = button.colors;

        string correctAnswer = knowledgeCheckQuestions[currentQuestion][QuestionHeaderIndexes["Correct Answer(s)"]];

        // Convert the correct answer to a list of characters
        List<char> correctAnswerList = new List<char>(correctAnswer.ToCharArray());

        // Assuming we need to check the entire button text against the correct answer
        bool isCorrect = correctAnswerList.Contains(buttonText.text[0]);

        if (isCorrect)
        {
            buttonImage.color = colors.selectedColor;
            Debug.Log("correct");
            questionsAnswered += 1;
            
        }
        else
        {
            buttonImage.color = colors.disabledColor;
            giveHint(button.gameObject.name);
        }

        Collider collider = button.GetComponent<Collider>();
        collider.enabled = false;
    }

    private IEnumerator Wait(float secondsWaiting)
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(secondsWaiting);

        // Deactivate the game object after waiting
        
    }

    // Optional method to give hints
    private void giveHint(string answerName)
    {
        Debug.Log("Hint for " + answerName);
        // Implement hint logic here
    }

    public void testButtonClick()
    {
        Debug.Log("click");   
    }

    public int getNumAnswered(){
    	return questionsAnswered;
	}

    public void deactivateQuestionPanel()
    {
        Wait(2f);
        gameObject.SetActive(false);
        hud_text.gameObject.SetActive(true);
    }
}
