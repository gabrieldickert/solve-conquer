using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public MovingPlatform platform;
    public int mode = 0;
    public GameObject VisualTrigger1;
    public GameObject VisualTrigger2;
    private bool playerOnPlatform = false;
    private bool companionOnPlatform = false;

    private void Start(){
        switch(mode){
            case 0: 
                VisualTrigger1.GetComponent<MeshRenderer>().enabled = false;
                VisualTrigger2.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 1:
                VisualTrigger2.GetComponent<MeshRenderer>().enabled = false;
                break;
        }
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Companion"){
            companionOnPlatform = true;
        }
        else if(other.gameObject.tag == "Player"){
            playerOnPlatform = true;
        }
        switch(mode){
            case 0: 
                platform.NextPlatform();
                break;
            case 1:
                if (playerOnPlatform){
                    platform.NextPlatform();
                } 
                break;
            case 2:
                if (playerOnPlatform && companionOnPlatform){
                    platform.NextPlatform();
                }
                break;
        }
    }
}
