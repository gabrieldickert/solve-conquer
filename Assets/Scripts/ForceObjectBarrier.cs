using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class ForceObjectBarrier : MonoBehaviour
{


    public GameObject LeftHand;
    public GameObject RightHand;



    private OVRGrabber LeftHandGrabber;
    private OVRGrabber RightHandGrabber;


    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Debug.Log("ForceObjectBarrier: Start");
        this.LeftHandGrabber = this.LeftHand.GetComponent<DistanceGrabber>();
        this.RightHandGrabber = this.RightHand.GetComponent<DistanceGrabber>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //UnityEngine.Debug.Log("ForceObjectBarrier: Player entered TriggerZone");
            if (this.LeftHandGrabber.grabbedObject != null)
            {
                this.LeftHandGrabber.ForceRelease(this.LeftHandGrabber.grabbedObject);
            }
            if (this.RightHandGrabber.grabbedObject != null)
            {
                this.RightHandGrabber.ForceRelease(this.RightHandGrabber.grabbedObject);
            }
        }
        if (other.gameObject.tag == "GrabbableObject")
        {
            if (this.LeftHandGrabber.grabbedObject == null && this.RightHandGrabber.grabbedObject == null)
            {
                //UnityEngine.Debug.Log("ForceObjectBarrier: GrabbableObject entered TriggerZone");
                //ResetObjectPosition(other.gameObject.GetInstanceID());
                Reposition(other.gameObject);
                //Ensure dropping items when player passes barrier
            }
        }
        

    }

   

    void Reposition(GameObject targetGameObject)
    {
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
