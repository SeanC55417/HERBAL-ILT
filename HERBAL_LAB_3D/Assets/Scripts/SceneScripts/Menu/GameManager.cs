using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public string nextScene = "";

    public void LoadScene(string sceneName)
    {
        if (sceneName == "" && nextScene != ""){
            sceneName = nextScene;
        }
        SceneManager.LoadScene(sceneName);
    }


    // Call this method to quit the game
    public void QuitGame()
    {
        Debug.Log("Quit game requested");

        // Quit the application
        Application.Quit();

        // If running inside the Unity editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
