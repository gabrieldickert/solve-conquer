using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    //Sound 
    private AudioSource source;
    public AudioClip switchSound;

    private bool isActiveTrigger_2 = false;
    private bool switchHit = false;

    private float switchRotation = 100;

    public int triggerId_1;
    public int triggerId_2;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.spatialBlend = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (switchHit == true)
        {
            //PlaySound
            source.PlayOneShot(switchSound);
            switchHit = false;
            //if on is true make on false, anf if on is false make on true
            //on = !on;

            //rotate
            if (isActiveTrigger_2)
            {

                transform.rotation = Quaternion.Euler(transform.eulerAngles.x + switchRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x - switchRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("GrabVolumeBig"))
        {
            EventsManager.instance.OnSwitchEnable(isActiveTrigger_2 ? triggerId_1 : triggerId_2);
            EventsManager.instance.OnSwitchDisable(isActiveTrigger_2 ? triggerId_2 : triggerId_1);
            isActiveTrigger_2 = !isActiveTrigger_2;
            switchHit = true;
        }
    }
}
