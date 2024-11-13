using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

[System.Serializable]
public class NotebookScript : MonoBehaviour
{
    public Transform PagesParent;
    public GameObject pageContainer; // Parent transform for pages
    public TextMeshProUGUI pageTextPrefab; // TextMeshPro prefab for each page
    private Dictionary<string, string[]> typeOfTest = new Dictionary<string, string[]>();
    GameObject currentContainer;
    private int currentPageIndex = 0;
    private List<GameObject> pages = new List<GameObject>();
    private float contentHeight = 0;
    private int maxPageIndex = -1;
    [SerializeField]
    public GameObject solventPrefab;
    public GameObject prefabContainer;
    List<string> waitingForStudentList = new List<string>();
    bool deletePriors = false;
    public GameObject ElutionPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        InstantiateContainer();
        typeOfTest["solventPrefab"] = new string[] {"MiscibilityTest.solvent1", "MiscibilityTest.solvent2", "Phenomenex Kinetex C18 (4.6 x 150 mm) 100 Å", "0.4 ml/min", "30C", "254 nm"};
        typeOfTest["Elution Image"] = new string[] {};
        // postText("Welcome to the Notebook. Here is where your data will be recorded.\n");
    }

    public void button1()
    {
        postText("Instrument Parameters");
    }
    public void button2()
    {
        postText("\nPolar Sol. A: ");
    }
    public void button3()
    {
        postText("\nOrganic Sol. B: ");
    }
    public void button4()
    {
        postText("\nColumn: Phenomenex Kinetex C18 (4.6 x 150 mm) 100 Å");
    }
    public void button5()
    {
        postText("\nFlow Rate: 0.4ml/min");
    }
    public void button6()
    {
        postText("\nOven Temp: 30C");
    }
    public void button7()
    {
        postText("\nObserved Wavelength: 254 nm");
    }
    public void button8()
    {
        postText("\nElution Pattern");
    }
    public void button9()
    {
        postPrefab(ElutionPrefab);
    }



    void InstantiateContainer()
    {
        currentContainer = Instantiate(pageContainer, PagesParent);
        pages.Add(currentContainer);
        maxPageIndex++;
        contentHeight = 0;
        flipMaxPage();
    }

    void InstantiateTextBox()
    {
        Instantiate(pageTextPrefab, currentContainer.transform);
    }

    public void postText(string textToAdd)
    {
        bool overflow = false;
        do
        {
            // if (currentContainer == null) {
            //     InstantiateTextBox();
            // }
            int lastChildIndex = currentContainer.transform.childCount - 1;
            
            if (lastChildIndex == -1)
            {
                InstantiateTextBox();
                lastChildIndex = currentContainer.transform.childCount - 1;
            }

            if (lastChildIndex >= 0)
            {
                Transform lastChild = currentContainer.transform.GetChild(lastChildIndex);

                if (lastChild.CompareTag("textBox"))
                {
                    TextMeshProUGUI textBox = lastChild.GetComponent<TextMeshProUGUI>();
                    textBox.text += textToAdd;

                    float sizeDifference = textBox.preferredHeight - textBox.rectTransform.sizeDelta.y;
                    textBox.rectTransform.sizeDelta = new Vector2(textBox.rectTransform.sizeDelta.x, textBox.preferredHeight);

                    if (checkOverflow())
                    {
                        Debug.Log("Overflow True");
                        textBox.text = textBox.text.Remove(textBox.text.Length - textToAdd.Length);
                        InstantiateContainer();
                        overflow = true;
                    }
                    else
                    {
                        overflow = false;
                    }
                } 
                else 
                {
                    InstantiateTextBox();
                    overflow = true;
                }
            }
        } while (overflow == true);
    }

    void postPrefab(GameObject prefab)
    {
        List<GameObject> nextPageChildrenList = new List<GameObject>();
        
        float parentPrefabContainerHeight = 0;
        float spaceAvailable = currentContainer.GetComponent<RectTransform>().sizeDelta.y - contentHeight;
        
        string[] prefabUpdateText = typeOfTest[prefab.name];
        int updatedTextCount = 0;
        int startingIndex = -1; // Initialize startingIndex to -1 to indicate no match found initially
        
        // Instantiate the prefab within the current container
        GameObject instantiatedPrefab = Instantiate(prefab, currentContainer.transform);
        RectTransform prefabContainerRectTransform = instantiatedPrefab.GetComponent<RectTransform>();

        // Ensure prefabContainer is instantiated before the loop
        if (prefabContainer == null)
        {
            prefabContainer = Instantiate(prefabContainer, currentContainer.transform);
            prefabContainer.name = prefab.name;
        }

        if (waitingForStudentList.Count > 0) 
        {
            bool matchFound = false; // Flag to indicate if a match is found
            for (int j = 0; j < waitingForStudentList.Count; j++)
            {
                for (int k = 0; k < instantiatedPrefab.transform.childCount; k++)
                {
                    // Debug.Log("WaitingForStudentList[j]: " + waitingForStudentList[j] + " instantiatePrefab Child: " + instantiatedPrefab.transform.GetChild(k).gameObject.name);
                    if (waitingForStudentList[j] == instantiatedPrefab.transform.GetChild(k).gameObject.name)
                    {
                        deletePriors = true;
                        startingIndex = k;
                        matchFound = true; // Set the flag to true
                        break; // Exit the inner loop
                    }
                }
                if (matchFound)
                {
                    break; // Exit the outer loop if a match is found
                }
            }
        }

        for (int i = 0; i < instantiatedPrefab.transform.childCount; i++)
        {
            Transform childTransform = instantiatedPrefab.transform.GetChild(i);
            GameObject childGameObject = childTransform.gameObject;
            RectTransform childRectTransform = childTransform.GetComponent<RectTransform>();

            // Debug.Log("startingIndex: " + startingIndex);

            if (childGameObject.CompareTag("UpdateText") && prefabUpdateText[updatedTextCount] == null)
            {
                for (int o = i; o < instantiatedPrefab.transform.childCount; o++)
                {
                    childTransform = instantiatedPrefab.transform.GetChild(o);
                    childGameObject = childTransform.gameObject;
                    waitingForStudentList.Add(childGameObject.name);
                    Debug.Log(childGameObject.name);
                    Destroy(childGameObject);
                }
                break;
            }

            if (deletePriors && startingIndex >= 0)
            {
                for (int k = startingIndex; k >= 0; k--)
                {
                    Destroy(instantiatedPrefab.transform.GetChild(k).gameObject);
                }
                deletePriors = false;
            }

            // Handle the update text logic
            // Debug.Log("updatedTextCount: " + updatedTextCount);
            if (childGameObject.CompareTag("UpdateText") && prefabUpdateText[updatedTextCount] != null)
            {
                TextMeshProUGUI childTextMeshPro = childTransform.GetComponent<TextMeshProUGUI>();

                // Make a copy of the original text and update it
                string originalText = childTextMeshPro.text;
                childTextMeshPro.text = originalText + prefabUpdateText[updatedTextCount];

                // Adjust the size of the rect transform if needed
                if (childTextMeshPro.preferredHeight > childRectTransform.sizeDelta.y) 
                {
                    childRectTransform.sizeDelta = new Vector2(childRectTransform.sizeDelta.x, childTextMeshPro.preferredHeight);
                }
                updatedTextCount++;
            }

            // Update the height of the parent prefab container
            parentPrefabContainerHeight += childRectTransform.sizeDelta.y;
            prefabContainerRectTransform.sizeDelta = new Vector2(prefabContainerRectTransform.sizeDelta.x, parentPrefabContainerHeight);
        }
        fixPrefabOverflow(instantiatedPrefab.transform);
    }

    void fixPrefabOverflow(Transform currentPrefab)
    {
        float currentContentHeight = 0;
        Transform tempContainer = currentContainer.transform;
        for (int i = 0; i < tempContainer.childCount; i++)
        {
            Transform childTransform = tempContainer.GetChild(i);
            RectTransform childRectTransform = childTransform.GetComponent<RectTransform>();

            if (childTransform.CompareTag("Prefab"))
            {
                float parentContainerHeight = 0;
                for (int k = 0; k < childTransform.childCount; k++)
                {

                    Debug.Log("k: " + k);
                    Transform prefabChildTransform = childTransform.GetChild(k);
                    RectTransform prefabChildRectTransform = prefabChildTransform.GetComponent<RectTransform>();

                    Debug.Log(prefabChildTransform.name);
                    currentContentHeight += prefabChildRectTransform.sizeDelta.y;
                    parentContainerHeight += prefabChildRectTransform.sizeDelta.y;
                    Debug.Log("Content Height: " + currentContentHeight + "Container Height" + tempContainer.GetComponent<RectTransform>().sizeDelta.y);
                    if (currentContentHeight > tempContainer.GetComponent<RectTransform>().sizeDelta.y)
                    {
                        currentContentHeight -= prefabChildRectTransform.sizeDelta.y;
                        parentContainerHeight -= prefabChildRectTransform.sizeDelta.y;
                        InstantiateContainer();
                        float currentPrefabHeight = 0;
                        GameObject currentPrefabContainer = Instantiate(prefabContainer, currentContainer.transform);
                        currentPrefabContainer.name = currentPrefab.name;
                        prefabChildTransform.SetParent(currentPrefabContainer.transform);

                        for (int j = k; j < childTransform.childCount; j++)
                        {
                            prefabChildTransform = childTransform.GetChild(j);
                            prefabChildTransform.SetParent(currentPrefabContainer.transform);
                            RectTransform newPrefabChildRectTransform = prefabChildTransform.GetComponent<RectTransform>();
                            currentPrefabHeight += newPrefabChildRectTransform.sizeDelta.y;
                        }
                        RectTransform currentPrefabContainerRect = currentPrefabContainer.GetComponent<RectTransform>();
                        currentPrefabContainerRect.sizeDelta = new Vector2(870, currentPrefabHeight);
                        childRectTransform.sizeDelta = new Vector2(870, parentContainerHeight);
                        k = childTransform.childCount;

                        if (childTransform.CompareTag("noSplit"))
                        {
                            int movePrior = k;
                            do
                            {
                                Transform priorChildTransform = childTransform.GetChild(movePrior);
                                if (!priorChildTransform.CompareTag("noSplit"))
                                {
                                    movePrior = -1;
                                    priorChildTransform.SetParent(currentPrefabContainer.transform);
                                }
                                else
                                {
                                    movePrior =- 1;
                                }
                            } while (movePrior != -1);
                        }
                    }
                }
            }
            else if (childRectTransform != null)
            {
                currentContentHeight += childRectTransform.sizeDelta.y;
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

        if (contentHeight > currentContainer.GetComponent<RectTransform>().sizeDelta.y)
        {
            return true;
        }
        return false;
    }

    // float getOverflowHeight()
    // {
    //     contentHeight = 0;
    //     int contentChildCount = currentContainer.transform.childCount;

    //     for (int i = 0; i < contentChildCount; i++)
    //     {
    //         RectTransform childRectTransform = currentContainer.transform.GetChild(i).GetComponent<RectTransform>();
    //         if (childRectTransform != null)
    //         {
    //             contentHeight += childRectTransform.sizeDelta.y;
    //         }
    //     }

    //     if (contentHeight > currentContainer.GetComponent<RectTransform>().sizeDelta.y)
    //     {
    //         return contentHeight - currentContainer.GetComponent<RectTransform>().sizeDelta.y;
    //     }
    //     return -1;
    // }

    public void postBulletPoints(params string[] bulletPoints)
    {
        string bullet = "\u2022";
        string bulletSet = $"<b>{bulletPoints[0]}</b>\n";
        postText(bulletSet);

        for (int i = 0; i < bulletPoints.Length - 1; i++)
        {
            postText($"{bullet} {bulletPoints[i + 1]} \n");
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

    public void HPLCPrefabs()
    {
        typeOfTest["solventPrefab"] = new string[] {"MiscibilityTest.solvent1", "MiscibilityTest.solvent2", "Phenomenex Kinetex C18 (4.6 x 150 mm) 100 Å", "0.4 ml/min", "30C", "254 nm"};
        postPrefab(solventPrefab);
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            postText($"<b>Purpose:</b> In this lab we will compare the relative abundance of taxol using a standard in three different extracts from the bark, fruit, and leaves of the Pacific Yew Tree. \n");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            postNumberedBullets("Major Steps", "Set-up instrument with solvent system and column", "Equilibrate system", "Inject samples", "Analyze chromatograms");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            postPrefab(solventPrefab);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            postBulletPoints("Timeline:", "ASAP 1: Make plates, plate 15 strains", "ASAP 2: Inoculate strains", "ASAP 3: Plate, OD, hemocytometer", "ASAP 4: Count plates");
        }
    }
}

