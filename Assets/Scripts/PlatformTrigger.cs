using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    /*public MovingPlatform platform;
    private void OnTriggerEnter(Collider other){
        platform.NextPlatform();
    }*/

    public MovingPlatform platform;
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Companion"){
            platform.NextPlatform();
        }
    }
}
