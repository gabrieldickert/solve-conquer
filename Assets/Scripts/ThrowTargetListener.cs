using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTargetListener : MonoBehaviour
{

    public bool isActive = false;
    public int trigger1 = 0;
    public int trigger2 = 0;

    public MeshRenderer lightRenderer1 = null;
    public MeshRenderer lightRenderer2 = null;

    public Sound SoundSrc;
    private AudioSource AudioSrc;

    // Start is called before the first frame update
    void Start()
    {
        lightRenderer1.material.color = Color.red;
        lightRenderer2.material.color = Color.red;
        this.AudioSrc = this.gameObject.GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Throwable")
        {
            if (!isActive)
            {
                lightRenderer1.material.color = Color.green;
                lightRenderer2.material.color = Color.green;
                EventsManager.instance.OnThrowableTargetEnable(trigger1);
                EventsManager.instance.OnThrowableTargetEnable(trigger2);
                isActive = true;

                //this.AudioSrc.clip = this.SoundSrc.clipList[0];
                this.AudioSrc.PlayOneShot(this.SoundSrc.clipList[0]);
            }
            else if (isActive)
            {
                lightRenderer1.material.color = Color.red;
                lightRenderer2.material.color = Color.red;
                EventsManager.instance.OnThrowableTargetDisable(trigger1);
                EventsManager.instance.OnThrowableTargetDisable(trigger2);
                // this.AudioSrc.clip = this.SoundSrc.clipList[1];
                this.AudioSrc.PlayOneShot(this.SoundSrc.clipList[1]);
                isActive = false;
            }
        }
    }
}
