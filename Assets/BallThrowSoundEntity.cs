using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class BallThrowSoundEntity : MonoBehaviour
{
    public Sound SoundSrc;
    private AudioSource AudioSrc;
    private Rigidbody Physics;
    private DistanceGrabbable DistanceGrab;
    // Start is called before the first frame update
    void Start()
    {
        this.AudioSrc = this.gameObject.GetComponent<AudioSource>();
        this.Physics = this.gameObject.GetComponent<Rigidbody>();
        EventsManager.instance.PlayThrowSound += HandlePlayThrowSound;
       // this.DistanceGrab = this.gameObject.GetComponent<DistanceGrabbable>();


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
       /* this.AudioSrc.volume = 1;
        this.AudioSrc.clip = this.SoundSrc.clipList[1];
        this.AudioSrc.Play();*/
        

      
    }

    private void HandlePlayThrowSound(int gameObjectId)
    {
        if(gameObjectId == gameObject.GetInstanceID())
        {
            this.AudioSrc.PlayOneShot(this.SoundSrc.clipList[0]);
        }
    }
}
