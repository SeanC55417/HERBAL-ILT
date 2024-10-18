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
    public GameObject QuestionPanel;
    public GameObject answerPrefab;
    public GameObject Question;
    
    // Add these fields to track progress and objects
    private int questionsAnswered = 0;
    private GameObject currentQuestion; // Reference for current question

    void Start()
    {
        getQuestions();
    }

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
        float currentQuestionHeight = 0;
        currentQuestion = Instantiate(Question, QuestionPanel.transform); // Assigning current question

        if (knowledgeCheckQuestions.ContainsKey(questionKey))
        {
            TextMeshProUGUI QuestionTextMesh = currentQuestion.GetComponent<TextMeshProUGUI>();
            RectTransform QuestionRectTransform = currentQuestion.GetComponent<RectTransform>();

            QuestionTextMesh.text = knowledgeCheckQuestions[questionKey][0];

            QuestionRectTransform.sizeDelta = new Vector2(QuestionRectTransform.sizeDelta.x, QuestionTextMesh.preferredHeight);
            currentQuestionHeight += QuestionRectTransform.sizeDelta.y;
            currentQuestion.name = questionKey;

            // Loop through answer options
            for (int i = 1; i < knowledgeCheckQuestions[questionKey].Length - 2; i++)
            {
                if (knowledgeCheckQuestions[questionKey][i] != "" && i < QuestionHeaderIndexes["Hint"])
                {
                    GameObject currentAnswerOption = Instantiate(answerPrefab, currentQuestion.transform); // Instantiate answerPrefab
                    Button button = currentAnswerOption.GetComponent<Button>();
                    TextMeshProUGUI childTextMeshPro = currentAnswerOption.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    RectTransform childRectTransform = currentAnswerOption.GetComponent<RectTransform>();
                    BoxCollider boxCollider = currentAnswerOption.GetComponent<BoxCollider>();
                    button.onClick.AddListener(() => checkAnswer(button, childTextMeshPro));
                    currentAnswerOption.name = "Answer " + i;

                    childTextMeshPro.text = knowledgeCheckQuestions[questionKey][i];
                    if (childTextMeshPro.preferredHeight > childRectTransform.sizeDelta.y)
                    {
                        childRectTransform.sizeDelta = new Vector2(childRectTransform.sizeDelta.x, childTextMeshPro.preferredHeight + 20);
                        boxCollider.size = new Vector2(childRectTransform.sizeDelta.x, childRectTransform.sizeDelta.y);
                        // currentAnswerOption. = new Vector2(childRectTransform.sizeDelta.x, childRectTransform.sizeDelta.y);
                    }
                    currentQuestionHeight += childRectTransform.sizeDelta.y + 5;
                }
            }

            RectTransform emptyRectTransform = currentQuestion.GetComponent<RectTransform>();
            emptyRectTransform.sizeDelta = new Vector2(emptyRectTransform.sizeDelta.x, currentQuestionHeight);
        }
    }

    public void checkAnswer(Button button, TextMeshProUGUI buttonText)
    {
        // Get the Image component attached to the button
        Image buttonImage = button.GetComponent<Image>();
        ColorBlock colors = button.colors;

        string correctAnswer = knowledgeCheckQuestions[currentQuestion.name][QuestionHeaderIndexes["Correct Answer(s)"]];

        // Convert the correct answer to a list of characters
        List<char> correctAnswerList = new List<char>(correctAnswer.ToCharArray());

        // Assuming we need to check the entire button text against the correct answer
        bool isCorrect = correctAnswerList.Contains(buttonText.text[0]);

        if (isCorrect)
        {
            buttonImage.color = colors.selectedColor;
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

    // Optional method to give hints
    private void giveHint(string answerName)
    {
        Debug.Log("Hint for " + answerName);
        // Implement hint logic here
    }
}
