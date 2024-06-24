using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string nextScene = "";

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName) && !string.IsNullOrEmpty(nextScene))
        {
            sceneName = nextScene;
        }
        if (!string.IsNullOrEmpty(sceneName))
        {
            LoadingScreen.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is empty!");
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quit game requested");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
