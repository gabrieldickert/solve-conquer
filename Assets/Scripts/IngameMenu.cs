using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public GameObject ingameMenu;

    public static bool showMenu = false;
   
    void Start()
    {
        ingameMenu.SetActive(false);
    }

    void Update()
    {
        
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
        if (!showMenu && OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            ingameMenu.SetActive(true);
            showMenu = true;
        } else if(showMenu && OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            ingameMenu.SetActive(false);
            showMenu = false;
        }
    }
}
