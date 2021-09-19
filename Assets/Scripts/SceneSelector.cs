using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    private void Awake()
    {
        string path = Application.persistentDataPath + "/savegame.dat";
        if (File.Exists(path))
        {
            SceneManager.LoadScene("PlanetScene", LoadSceneMode.Single);
        }
    }
}
