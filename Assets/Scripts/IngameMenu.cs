using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public GameObject ingameMenu;
    private GameObject laserPointer;
    public static bool showMenu = false;

   
    void Start()
    {
        this.laserPointer = GameObject.Find("LaserPointer");
        this.laserPointer.SetActive(false);
        ingameMenu.SetActive(false);
    }

    void Update()
    {
        
        // Debug in Unity
        if (!showMenu && Input.GetKeyDown(KeyCode.M))
        {
            ingameMenu.SetActive(true);
            this.laserPointer.SetActive(true);
            showMenu = true;
        } else if (showMenu && Input.GetKeyDown(KeyCode.M))
        {
            ingameMenu.SetActive(false);
            this.laserPointer.SetActive(false);
            showMenu = false;
        }

        // Für VR
        if (!showMenu && OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            ingameMenu.SetActive(true);
            this.laserPointer.SetActive(true);
            showMenu = true;
        } else if(showMenu && OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            ingameMenu.SetActive(false);
            this.laserPointer.SetActive(false);
            showMenu = false;
        }
    }
}
