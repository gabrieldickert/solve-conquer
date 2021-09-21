using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class BallThrowSoundEntity : MonoBehaviour
{
    public Sound SoundSrc;
    private bool isThrowing = false;
    private AudioSource AudioSrc;
    private Rigidbody Physics;
    private DistanceGrabbable DistanceGrab;
    // Start is called before the first frame update
    void Start()
    {
        this.AudioSrc = this.gameObject.GetComponent<AudioSource>();
        this.Physics = this.gameObject.GetComponent<Rigidbody>();
        
       // this.DistanceGrab = this.gameObject.GetComponent<DistanceGrabbable>();
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log("X:"+this.gameObject.GetComponent<Rigidbody>().velocity.x);
        Debug.Log("Y:" + this.gameObject.GetComponent<Rigidbody>().velocity.y);
        Debug.Log("Z:" + this.gameObject.GetComponent<Rigidbody>().velocity.z);*/

        //Debug.Log("Speed" + this.gameObject.GetComponent<Rigidbody>().velocity.magnitude);

        if(this.Physics.velocity.magnitude > 0) {

    
            //Ball has Speed
            isThrowing = true;


        }
    }
}
