using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public bool showMenu = false;
    public GameObject ingameMenu;

    void Start()
    {
        ingameMenu.SetActive(false);
    }
    void Update()
    {
        
        OVRInput.Update();

        // Debug in Unity
        if (!showMenu && Input.GetKeyDown(KeyCode.M))
        {
            ingameMenu.SetActive(true);
            showMenu = true;
        } else if (showMenu && Input.GetKeyDown(KeyCode.M))
        {
            ingameMenu.SetActive(false);
            showMenu = false;
        }

        // Für VR
        if (!showMenu && OVRInput.GetDown(OVRInput.Button.Start))
        {
            ingameMenu.SetActive(true);
            showMenu = true;
        } else if(showMenu && OVRInput.GetDown(OVRInput.Button.Start))
        {
            ingameMenu.SetActive(false);
            showMenu = false;
        }
    }
}
