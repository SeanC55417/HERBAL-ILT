using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nextScreen : MonoBehaviour
{
    public GameObject zoomedGraph;
    public GameObject graphShown;
    public Sprite[] sprites;

    public void numberedGraph()
    {
        gameObject.SetActive(false);
        zoomedGraph.SetActive(true);
    }

    public void TaxolGraph()
    {
        graphShown.GetComponent<Image>().sprite = sprites[0];
    }

    public void BarkGraph()
    {
        graphShown.GetComponent<Image>().sprite = sprites[1];
    }

    public void LeavesGraph()
    {
        graphShown.GetComponent<Image>().sprite = sprites[2];
    }

    public void FruitGraph()
    {
        graphShown.GetComponent<Image>().sprite = sprites[3];
    }
}
