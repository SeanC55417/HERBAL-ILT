using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TeleportForGargen : MonoBehaviour
{
    public string nextScene = "";

    public void LoadScene(string sceneName)
    {
        if (sceneName == "" && nextScene != ""){
            sceneName = nextScene;
        }
        SceneManager.LoadScene(sceneName);
    }


    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            LoadScene(nextScene);
        }
    }
}
