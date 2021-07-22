using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hackable : MonoBehaviour
{
    public int triggerId;

    private bool isHackedByCompanion = false;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.CompanionHackObject += HandleCompanionHackObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Companion" && this.isHackedByCompanion)
        {
            EventsManager.instance.OnHackableEnable(triggerId);
            //Debug.Log("got hacked: companion hit trigger");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Companion")
        {
            EventsManager.instance.OnHackableDisable(triggerId);
            //Debug.Log("got unhacked: companion left trigger");
        }
    }

    private void HandleCompanionHackObject(GameObject targetObject)
    {
        if(gameObject == targetObject)
        {
            this.isHackedByCompanion = true;
        } else
        {
            this.isHackedByCompanion = false;
        }
    }
}
