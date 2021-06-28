using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    //Sound 
    private AudioSource source;
    public AudioClip switchSound;

    private bool on = false;
    private bool switchHit = false;

    private float switchRotation = 100;

    private GameObject switchBase;

    public int triggerId;

    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        switchBase = transform.GetChild(0).gameObject;


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
            on = !on;

            //rotate
            if (on == true)
            {

                transform.rotation = Quaternion.Euler(transform.eulerAngles.x + switchRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }
            else
            {
                EventsManager.instance.OnSwitchDisable(triggerId);
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x - switchRotation, transform.eulerAngles.y, transform.eulerAngles.z);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventsManager.instance.OnSwitchEnable(triggerId);
            switchHit = true;
        }
    }
}
