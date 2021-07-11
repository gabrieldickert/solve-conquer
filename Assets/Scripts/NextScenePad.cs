using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScenePad : MonoBehaviour
{
    public bool loadSceneByName = false;
    public string sceneName = "";

    private bool isAlreadyLoading = false;

    private void LoadNextScene()
    {
        if(loadSceneByName)
        {
            //UnityEngine.Debug.Log("NextLevelPad: Trying to load scene named '" + sceneName + "'");
            SceneManager.LoadSceneAsync(sceneName);
        } else if (HasNextScene())
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        }
        
        isAlreadyLoading = true;
    }

    private bool HasNextScene()
    {
        int indexOfLastScene = SceneManager.sceneCountInBuildSettings - 1;
        return SceneManager.GetActiveScene().buildIndex < indexOfLastScene;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isAlreadyLoading)
        {
            LoadNextScene();
        }
    }
}