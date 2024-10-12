using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isVR = false;

    public bool IsVR
    {
        get { return isVR; }
    }

    void Start()
    {
        if (FindAnyObjectByType<XROrigin>())
        {
            isVR = true;
        }
    }

    public string nextScene = "";

    // public void LoadScene(string sceneName)
    // {
    //     if (string.IsNullOrEmpty(sceneName) && !string.IsNullOrEmpty(nextScene))
    //     {
    //         sceneName = nextScene;
    //     }
    //     if (!string.IsNullOrEmpty(sceneName))
    //     {
    //         LoadingScreen.LoadScene(sceneName);
    //     }
    //     else
    //     {
    //         Debug.LogError("Scene name is empty!");
    //     }
    // }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ResetScene()
    {
        // Get the active scene and reload it
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit game requested");
        Application.Quit();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
