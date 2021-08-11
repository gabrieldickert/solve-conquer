using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public MovingPlatform platform;
    public string triggerTag;
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == triggerTag){
            platform.NextPlatform();
        }
    }
}
