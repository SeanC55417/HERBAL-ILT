using UnityEngine;
using UnityEngine.UI;

public class IterateThroughGraphs : MonoBehaviour
{
    public Sprite Taxol;
    public Sprite Bark;
    public Sprite Leaves;
    public Sprite Fruit;

    private Sprite[] sprites;
    private int currentIndex = 0;
    private Image imageComponent;

    void Start()
    {
        // Initialize the array of sprites
        sprites = new Sprite[] { Taxol, Bark, Leaves, Fruit };
    }

    public void nextGraph()
    {
        // Increase the index
        currentIndex++;

        // Wrap around if the index exceeds the number of sprites
        if (currentIndex >= sprites.Length)
        {
            currentIndex = 0;
        }

        // Set the sprite
        GetComponent<SpriteRenderer>().sprite = sprites[currentIndex];
    }
}
