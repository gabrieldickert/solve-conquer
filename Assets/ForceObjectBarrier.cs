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
            //ResetObjectPosition(other.gameObject.GetInstanceID());
            Reposition(other.transform.position, other.gameObject);
        }
    }

    void Reposition(Vector3 respectivePos, GameObject targetGameObject)
    {
        //UnityEngine.Debug.Log("ForceObjectBarrier: "+gameObject.transform.position.x+","+ gameObject.transform.position.y + ","+ gameObject.transform.position.z);
        //targetGameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 1);
       // Vector3 relativePoint = gameObject.transform.InverseTransformPoint(0, 0, 0);



        //float speed = 1.0f;


        Vector3 targetDirectionLocal = gameObject.transform.InverseTransformPoint(targetGameObject.transform.position);

        if (targetDirectionLocal.z < 0)
        {
            Debug.Log("ForceObjectBarrier: Target hit left side. Moving target further to the left.");
            targetGameObject.transform.position = transform.TransformPoint(Vector3.back * 2);
        }
        else if (targetDirectionLocal.z > 0)
        {
            Debug.Log("ForceObjectBarrier: Target hit right side. Moving target further to the right.");
            targetGameObject.transform.position = transform.TransformPoint(Vector3.forward * 2);

        }
    }

    void ResetObjectPosition(int instanceId)
    {
        //Event feuern
        //UnityEngine.Debug.Log("ForceObjectBarrier: Reset instance " + instanceId);
        EventsManager.instance.OnResetObject(instanceId);
    }
}
