using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public bool showMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Start))
        {
            gameObject.SetActive(true);
            showMenu = true;
        }

        if(showMenu && OVRInput.GetDown(OVRInput.Button.Start))
        {
            gameObject.SetActive(false);
            showMenu = false;
        }
    }

}
