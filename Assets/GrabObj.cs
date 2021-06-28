using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
public class GrabObj : OVRGrabber
{
    // Start is called before the first frame update
  

    protected override void GrabBegin()
    {
        Debug.Log("ja");
        //base.GrabBegin();     //Calls original function

    }
}
