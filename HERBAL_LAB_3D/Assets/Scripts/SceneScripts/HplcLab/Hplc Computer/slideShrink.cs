using UnityEngine;
using UnityEngine.UI;

public class ShrinkObjectTowardsRight : MonoBehaviour
{
    private float transitionDuration = 111.0f; // Adjust this value to control the speed of shrinking
    private RectTransform rectTransform;
    private float transitionProgress;
    private bool isShrinking = false;
    private int currentIndex = 0;
    public GameObject graphImage;
    public GameObject nextScreen;
    public GameObject currentScreen;
    public Sprite[] sprites; // Declare this as public so you can assign sprites in the Inspector
    
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(111.38f, rectTransform.sizeDelta.y); // Adjusted sizeDelta to float
    }

    private void Update()
    {
        if (isShrinking && transitionProgress < transitionDuration && rectTransform.sizeDelta.x > 0)
        {
            transitionProgress += Time.deltaTime * 30;
            float newXSize = Mathf.Lerp(111.38f, 0f, transitionProgress / transitionDuration);
            rectTransform.sizeDelta = new Vector2(newXSize, rectTransform.sizeDelta.y);
        }
        else if (rectTransform.sizeDelta.x <= 0)
        {
            rectTransform.sizeDelta = new Vector2(111.38f, rectTransform.sizeDelta.y);
            transitionProgress = 0f;
            NextGraph(); // Call NextGraph() to switch sprite
            
        }
    }

    public void OnTriggerEnter()
    {
        if (!isShrinking)
        {
            isShrinking = true;
            transitionProgress = 0f;
        }
    }

    private void NextGraph()
    {
        // Increase the index
        currentIndex++;

        // Wrap around if the index exceeds the number of sprites
        if (currentIndex >= sprites.Length)
        {
            currentIndex = 0;
            isShrinking = false;
            currentScreen.SetActive(false);
            nextScreen.SetActive(true);
        }

        // Set the sprite
        graphImage.GetComponent<Image>().sprite = sprites[currentIndex];
    }
}
