using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class BallThrowSoundEntity : MonoBehaviour
{
    public Sound SoundSrc;
    private bool isThrowing = false;
    private bool HasFirstCol = false;
    private float refSpeed = 0;
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
        if(this.Physics.velocity.magnitude > 1) {

        }
        else
        {

            HasFirstCol = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        this.AudioSrc.Stop();
        /*
        if(!HasFirstCol) {

            refSpeed = this.Physics.velocity.magnitude;

            HasFirstCol = true;
        }
        */
        this.AudioSrc.volume = 1;
        this.AudioSrc.clip = this.SoundSrc.clipList[1];
        this.AudioSrc.Play();


      
    }
}
