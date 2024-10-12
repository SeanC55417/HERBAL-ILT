using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComputerScreens : MonoBehaviour
{
    public List<GameObject> computerScreens = new List<GameObject>();
    int currentScreenIndex = 0;

    public void selfButtonDelete(GameObject coverButton)
    {
        Destroy(coverButton);
    }

    public void nextScreen()
    {
        currentScreenIndex++;
        setScreen();
    }

    public void previousScreen()
    {
        currentScreenIndex--;
        setScreen();
    }

    public void setScreen()
    {
        Debug.Log("Current Screen index: " + currentScreenIndex);
        for (int i = 0; i < computerScreens.Count; i++)
    	{
        	computerScreens[i].SetActive(i == currentScreenIndex);
    	}
    }
}
