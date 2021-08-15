using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnListener : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Runtergefallen"+other.gameObject.name);

        if(other.gameObject.name.Equals("GrabManager"))
        {

            EventsManager.instance.OnResetPlayer();
        }
        /*
        switch(other.tag)
        {

            case "Player":
                EventsManager.instance.OnResetPlayer();
                break;

            case "Companion":
                EventsManager.instance.OnResetCompanion();
                break;

            default:
                EventsManager.instance.OnResetObject(other.GetInstanceID());
                break;


        }*/

    }
}
