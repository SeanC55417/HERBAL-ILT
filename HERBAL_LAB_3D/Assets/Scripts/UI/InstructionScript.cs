using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using TMPro;

public class InstructionScript : MonoBehaviour
{
	public GameObject Question;
	public GameObject QuestionAnswer;
	public Transform PagesParent;
	private GameObject currentQuestion;
	public GameObject pageContainer; // Parent transform for pages
	public TextMeshProUGUI pageTextPrefab; // TextMeshPro prefab for each page
	private Dictionary<string, string[]> knowledgeCheckQuestions = new Dictionary<string, string[]>();
	GameObject currentContainer;
	private int currentPageIndex = 0;
	private List<GameObject> pages = new List<GameObject>();
	private float contentHeight = 0;
	private int maxPageIndex = -1;
	public GameObject QuestionEmptyObject;
	private Dictionary<string, int> QuestionHeaderIndexes = new Dictionary<string, int>();
	float currentQuestionHeight;
	bool currentlyAnsweringQuestion = false;
	string studentID = "student1";
	private List<string> hintsAlreadyGiven = new List<string>();
	private int questionsAnswered = 0;
	// public Sprite uncheckedCheckBox;
	// public Sprite checkedCheckBox;

	void Start()
	{
    	CreateContainer();
    	// postText("Here you will be trained to learn how to navigate the scene. \nUse your keyboard to move, and your mouse to pick up and place the soda cans into the refrigerator. \nUse scroll wheel to  adjust the object's distance. \nPress space to reset the object's orientation.\n");  
    	getQuestions();  
	}

	void order()
	{
    	postBulletPoints("Procedure", "Locate the solvent cabinet", "-Hint: flammable, yellow");
    	postBulletPointWithTab("Select the polar solvent and bring over to the A slot in the rack atop the HPLC", "-Hint: aqueous");
    	postBulletPointWithTab("Select the organic solvent from the cabinet and move to the B slot in the rack above the HPLC", "-Hint: CH3OH");
    	postBulletPointWithTab("Equip the HPLC with the C18 column from the drawer below the computer", "-Hint: Pre-Column comes first");
    	postKnowledgeCheck("Question1");
    	postKnowledgeCheck("Question2");
    	postBulletPoints("Procedure (cont.)", "Set your instrument parameters by adjusting the flow rate (0.4mL/min), the starting B concentration (20%), the oven temperature (23\u00B0C), and the wave;ength to 254 nm", "-Hint: reveal values hidden under boxes");
    	postBulletPointWithTab("Start the pump to equailibrate your column");
    	postKnowledgeCheck("Question3");
    	postKnowledgeCheck("Question4");
    	postBulletPoints("Procedure:", "Set up a quick batch using the standard method for each sample: Bark, Leaves, Fruit, and your Taxol Standard");
    	postBulletPointWithTab("Place your sample vials in the corresponding rack positions");
    	postBulletPointWithTab("press start quick batch");
    	postKnowledgeCheck("Question5");
    	postKnowledgeCheck("Question6");
    	//clock
    	postBulletPoints("Procedure", "Navigate between the tabs containing the chromatograms of your sample to answer the following questions");
    	postKnowledgeCheck("Question7");
    	postKnowledgeCheck("Question8");
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

	bool checkOverflow()
	{
    	contentHeight = 0;
    	int contentChildCount = currentContainer.transform.childCount;

    	for (int i = 0; i < contentChildCount; i++)
    	{
        	RectTransform childRectTransform = currentContainer.transform.GetChild(i).GetComponent<RectTransform>();
        	if (childRectTransform != null)
        	{
            	contentHeight += childRectTransform.sizeDelta.y;
        	}
    	}

    	// Debug.Log("ContentHeight: " + contentHeight + " Page Height: " + currentContainer.GetComponent<RectTransform>().sizeDelta.y);
    	if (contentHeight > currentContainer.GetComponent<RectTransform>().sizeDelta.y)
    	{
        	return true;
    	}
    	return false;
	}

	public void flipMaxPage()
	{
    	for (int i = 0; i < pages.Count; i++)
    	{
        	pages[i].SetActive(i == maxPageIndex);
    	}
	}

	public void flipPage()
	{
    	for (int i = 0; i < pages.Count; i++)
    	{
        	pages[i].SetActive(i == currentPageIndex);
    	}
	}

	public void nextPage()
	{
    	if (currentPageIndex + 1 < pages.Count)
    	{
        	currentPageIndex++;
        	flipPage();
    	}
	}

	public void previousPage()
	{
    	currentPageIndex = Mathf.Max(0, currentPageIndex - 1);
    	flipPage();
	}

	void CreateContainer()
	{
    	currentContainer = Instantiate(pageContainer, PagesParent);
    	pages.Add(currentContainer);
    	maxPageIndex++;
    	contentHeight = 0;
    	flipMaxPage();
	}

	public void setProperParentForTextBox(GameObject parentContainer)
	{
    	int lastChildIndex = parentContainer.transform.childCount - 1;
    	instantiateTextBoxIfNeeded(lastChildIndex, parentContainer);
	}

	public void setProperParentForTextBox()
	{
    	int lastChildIndex = currentContainer.transform.childCount - 1;
    	instantiateTextBoxIfNeeded(lastChildIndex, currentContainer);
	}

	public void instantiateTextBoxIfNeeded(int lastChildIndex, GameObject parentContainer)
	{
    	bool instantiateNewTexbox = false;
    	if (lastChildIndex == -1)
    	{
        	instantiateNewTexbox = true;
    	}
    	else if (lastChildIndex >= 0)
    	{
        	Transform lastChild = parentContainer.transform.GetChild(lastChildIndex);

        	if (lastChild.CompareTag("textBox"))
        	{
            	return;
        	}
        	else
        	{
            	instantiateNewTexbox = true;
        	}
    	}
    	else
    	{
        	instantiateNewTexbox = true;
    	}
    	if (instantiateNewTexbox)
    	{
        	Instantiate(pageTextPrefab, parentContainer.transform);
    	}
	}

	public void postText(string textToAdd)
	{
    	bool overflow = false;
    	do
    	{
        	setProperParentForTextBox();
        	Transform lastChild = currentContainer.transform.GetChild(currentContainer.transform.childCount - 1);

        	TextMeshProUGUI textBox = lastChild.GetComponent<TextMeshProUGUI>();
        	textBox.text += textToAdd;

        	float sizeDifference = textBox.preferredHeight - textBox.rectTransform.sizeDelta.y;
        	textBox.rectTransform.sizeDelta = new Vector2(textBox.rectTransform.sizeDelta.x, textBox.preferredHeight);

        	if (checkOverflow())
        	{
            	textBox.text = textBox.text.Remove(textBox.text.Length - textToAdd.Length);
            	CreateContainer();
            	overflow = true;
        	}
        	else
        	{
            	overflow = false;
        	}
    	} while (overflow == true);
	}

	public void postBulletPoints(params string[] bulletPoints)
	{
    	if (bulletPoints.Length == 0) return;

    	// Post the first bullet point in bold
    	string firstBullet = $"<b>{bulletPoints[0]}</b>\n";
    	postText(firstBullet);

    	// Post the rest of the bullet points
    	List<string> remainingBullets = new List<string>();
    	for (int i = 1; i < bulletPoints.Length; i++)
    	{
        	remainingBullets.Add(bulletPoints[i]);
    	}
    	postBulletPointWithTab(remainingBullets);
	}

	// Posts a bullet point without a header and you can specify how much indent you want (0-1)
	public void postBulletPointWithTab(List<string> bulletPoints)
	{
    	string bullet = "\u2022"; // Main bullet point symbol
    	string subBullet = "\u25E6"; // Sub-bullet point symbol

    	for (int i = 0; i < bulletPoints.Count; i++)
    	{
        	string point = bulletPoints[i];

        	// Check if the bullet point is a sub-point (starts with '-')
        	if (point.StartsWith("-"))
        	{
            	point = point.Substring(1).Trim(); // Remove leading '-' and trim spaces
            	postText($"\t{subBullet} {point}\n"); // Post as a sub-bullet point
        	}
        	else
        	{
            	postText($"{bullet} {point}\n"); // Post as a regular bullet point
        	}
    	}
	}

	public void postBulletPointWithTab(params string[] bulletPoints)
	{
    	string bullet = "\u2022"; // Main bullet point symbol
    	string subBullet = "\u25E6"; // Sub-bullet point symbol

    	for (int i = 0; i < bulletPoints.Length; i++)
    	{
        	string point = bulletPoints[i];

        	// Check if the bullet point is a sub-point (starts with '-')
        	if (point.StartsWith("-"))
        	{
            	point = point.Substring(1).Trim(); // Remove leading '-' and trim spaces
            	postText($"\t{subBullet} {point}\n"); // Post as a sub-bullet point
        	}
        	else
        	{
            	postText($"{bullet} {point}\n"); // Post as a regular bullet point
        	}
    	}
	}

	public void postNumberedBullets(params string[] numberedPoints)
	{
    	postText($"<b>{numberedPoints[0]} </b>\n");
    	for (int i = 0; i < numberedPoints.Length - 1; i++)
    	{
        	postText($"{i + 1}. {numberedPoints[i + 1]} \n");
    	}
	}

	public void postKnowledgeCheck(string questionName)
	{
    	if (currentlyAnsweringQuestion == false)
    	{
        	currentQuestion = Instantiate(QuestionEmptyObject, currentContainer.transform);
        	currentQuestionHeight = 0;
        	GameObject QuestionText = Instantiate(Question, currentQuestion.transform);
        	hintsAlreadyGiven.Clear();

        	if (knowledgeCheckQuestions.ContainsKey(questionName))
        	{
            	TextMeshProUGUI QuestionTextMesh = QuestionText.GetComponent<TextMeshProUGUI>();
            	RectTransform QuestionRectTransform = QuestionText.GetComponent<RectTransform>();

            	QuestionTextMesh.text = knowledgeCheckQuestions[questionName][0];
           	 
            	QuestionRectTransform.sizeDelta = new Vector2(QuestionRectTransform.sizeDelta.x, QuestionTextMesh.preferredHeight);
            	currentQuestionHeight += QuestionRectTransform.sizeDelta.y;
            	currentQuestion.name = questionName;
           	 
            	for (int i = 1; i < knowledgeCheckQuestions[questionName].Length - 2; i++)
            	{
                	if (knowledgeCheckQuestions[questionName][i] != "" && i < QuestionHeaderIndexes["Hint"])
                	{
                    	GameObject currentAnswerOption = Instantiate(QuestionAnswer, currentQuestion.transform);
                    	Button button = currentAnswerOption.GetComponent<Button>();
                    	TextMeshProUGUI childTextMeshPro = currentAnswerOption.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    	RectTransform childRectTransform = currentAnswerOption.GetComponent<RectTransform>();
                    	BoxCollider boxCollider = currentAnswerOption.GetComponent<BoxCollider>();
                    	button.onClick.AddListener(() => checkAnswer(button, childTextMeshPro));
                    	currentAnswerOption.name = "Answer " + i;
                   	 
                    	childTextMeshPro.text = knowledgeCheckQuestions[questionName][i];
                    	if (childTextMeshPro.preferredHeight > childRectTransform.sizeDelta.y)
                    	{
                        	childRectTransform.sizeDelta = new Vector2(childRectTransform.sizeDelta.x, childTextMeshPro.preferredHeight + 20);
                        	boxCollider.size = new Vector2(childRectTransform.sizeDelta.x, childRectTransform.sizeDelta.y);
                    	}
                    	currentQuestionHeight += childRectTransform.sizeDelta.y + 5;
                	}
            	}

            	RectTransform emptyRectTransform = currentQuestion.GetComponent<RectTransform>();
            	emptyRectTransform.sizeDelta = new Vector2(emptyRectTransform.sizeDelta.x, currentQuestionHeight);

            	if (checkOverflow())
            	{
                	CreateContainer();
                	currentQuestion.transform.SetParent(currentContainer.transform);
            	}
        	}
        	else
        	{
            	// Debug.Log("Question not found inside dictionary");
        	}
    	}
	}

	public void giveHint(string buttonName)
	{
    	setProperParentForTextBox(currentQuestion);
    	int hintIndex = QuestionHeaderIndexes["Hint"] + (int)(buttonName[buttonName.Length - 1] - '0') - 1;
    	TextMeshProUGUI hint = currentQuestion.transform.GetChild(currentQuestion.transform.childCount - 1).GetComponent<TextMeshProUGUI>();
    	// Get the questionAnswer relative index and loop set the hintIndex to the relative index ex. hint index + 3 (if the user presses the 3rd answer)
    	string hintText = knowledgeCheckQuestions[currentQuestion.name][hintIndex];
    	if (!hintsAlreadyGiven.Contains(hintText))
    	{
        	hint.text += hintText + "\n";
        	hintsAlreadyGiven.Add(hintText);
        	RectTransform hintRectTransform = hint.GetComponent<RectTransform>();
        	hintRectTransform.sizeDelta = new Vector2(hintRectTransform.sizeDelta.x, hint.preferredHeight);
        	currentQuestionHeight += hint.preferredHeight;
        	RectTransform emptyRectTransform = currentQuestion.GetComponent<RectTransform>();
        	emptyRectTransform.sizeDelta = new Vector2(emptyRectTransform.sizeDelta.x, currentQuestionHeight);

        	if (checkOverflow())
        	{
            	CreateContainer();
            	currentQuestion.transform.SetParent(currentContainer.transform);
        	}
    	}
	}

	public void checkAnswer(Button button, TextMeshProUGUI buttonText)
	{
    	// Get the Image component attached to the button
    	Image buttonImage = button.GetComponent<Image>();
    	ColorBlock colors = button.colors;

    	// Debug.Log("CAI: " + correctAnswerIndex);
    	string correctAnswer = knowledgeCheckQuestions[currentQuestion.name][QuestionHeaderIndexes["Correct Answer(s)"]];

    	// Convert the correct answer to a list of characters
    	List<char> correctAnswerList = new List<char>(correctAnswer.ToCharArray());

    	// Assuming we need to check the entire button text against the correct answer
    	bool isCorrect = correctAnswerList.Contains(buttonText.text[0]);

    	if (isCorrect)
    	{
        	// Debug.Log("Correct answer: " + buttonText.text[0]);
        	buttonImage.color = colors.selectedColor;
        	questionsAnswered += 1;
    	}
    	else
    	{
        	// Debug.Log("Incorrect answer: " + buttonText.text[0]);
        	buttonImage.color = colors.disabledColor;
        	giveHint(button.gameObject.name);
    	}
    	Collider collider = button.GetComponent<Collider>();
    	collider.enabled = false;
    	// writeToFile("Student Answer: " + buttonText.text[0], studentID);
	}


	// public void checkbox()
	// {
	// 	GameObject emptyObject = Instantiate(QuestionEmptyObject, currentContainer.transform);

	// 	Sprite checkbox = Instantiate(uncheckedCheckBox, emptyObject.transform);
   	 
	// }

	public void writeToFile(string textToAdd, string fileName)
	{
    	string directoryPath = "Assets/Final Notebook Assets/StudentCSVs/" + fileName;
    	using (StreamWriter writer = new StreamWriter(directoryPath, true))
    	{
        	writer.WriteLine(textToAdd);
    	}
	}

	public int getNumAnswered(){
    	return questionsAnswered;
	}

	public void Update()
	{
    	if (Input.GetKeyDown(KeyCode.Alpha1))
    	{
        	order();
    	}
    	// else if (Input.GetKeyDown(KeyCode.Alpha2))
    	// {
    	// 	nextPage();
    	// 	flipPage();
    	// }
    	if (Input.GetKeyDown(KeyCode.Alpha3))
    	{
        	postText($"This is example text that will be replaced eventually \n");
    	}
    	else if (Input.GetKeyDown(KeyCode.Alpha4))
    	{
        	postKnowledgeCheck("Question1");
    	}
	}
}
