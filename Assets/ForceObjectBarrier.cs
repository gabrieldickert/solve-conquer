using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceObjectBarrier : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Debug.Log("ForceObjectBarrier: Start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("ForceObjectBarrier: " + other.gameObject.name);
        if(other.gameObject.tag == "GrabbableObject")
        {
            //UnityEngine.Debug.Log("ForceObjectBarrier: Suitable object found");
            ResetObjectPosition(other.gameObject.GetInstanceID());
            
        }
    }

    void ResetObjectPosition(int instanceId)
    {
        //Event feuern
        //UnityEngine.Debug.Log("ForceObjectBarrier: Reset instance " + instanceId);
        EventsManager.instance.OnResetObject(instanceId);
    }
}
