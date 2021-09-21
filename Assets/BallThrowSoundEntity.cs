using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowSoundEntity : MonoBehaviour
{
    public Sound SoundSrc;
    private bool isThrowing = false;
    private AudioSource AudioSrc;
    // Start is called before the first frame update
    void Start()
    {
        this.AudioSrc = this.gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
