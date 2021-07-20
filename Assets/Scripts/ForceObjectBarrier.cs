using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class ForceObjectBarrier : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public AudioClip sound_objectBlocked;
    
    private AudioSource source;
    private OVRGrabber LeftHandGrabber;
    private OVRGrabber RightHandGrabber;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Debug.Log("ForceObjectBarrier: Start");
        this.LeftHandGrabber = this.LeftHand.GetComponent<DistanceGrabber>();
        this.RightHandGrabber = this.RightHand.GetComponent<DistanceGrabber>();
        this.source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.tag == "Player" && (this.LeftHandGrabber.grabbedObject != null || this.RightHandGrabber.grabbedObject != null)) || other.gameObject.tag == "GrabbableObject")
        {
            this.source.PlayOneShot(sound_objectBlocked);
        } 
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //UnityEngine.Debug.Log("ForceObjectBarrier: Player entered TriggerZone");
            
            if (this.LeftHandGrabber.grabbedObject != null)
            {
                OVRGrabbable grabbedLeft = this.LeftHandGrabber.grabbedObject;
                this.LeftHandGrabber.ForceRelease(grabbedLeft);
                Reposition(grabbedLeft.gameObject);
            }
            if (this.RightHandGrabber.grabbedObject != null)
            {
                OVRGrabbable grabbedRight = this.RightHandGrabber.grabbedObject;
                this.RightHandGrabber.ForceRelease(grabbedRight);
                Reposition(grabbedRight.gameObject);
            }
        }
        else if (other.gameObject.tag == "GrabbableObject")
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
        //Stop previously induced movement
        Rigidbody rigidBody = targetGameObject.GetComponent<Rigidbody>();
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
        
        Vector3 targetDirectionLocal = gameObject.transform.InverseTransformPoint(targetGameObject.transform.position);
        Vector3 localBack = new Vector3(targetDirectionLocal.x, targetDirectionLocal.y, -1);
        Vector3 localForward = new Vector3(targetDirectionLocal.x, targetDirectionLocal.y, 1);
          
        if (targetDirectionLocal.z < 0)
        {
            Vector3 forceVector = transform.TransformPoint(localBack) - targetGameObject.transform.position;
            //Debug.Log("ForceObjectBarrier: Target hit left side. Moving target further to the left.");
            //Debug.Log("ForceObjectBarrier: Applying force vector " + forceVector + ". Current object vector " + targetGameObject.transform.position);
            targetGameObject.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        }
        else if (targetDirectionLocal.z > 0)
        {
            Vector3 forceVector = transform.TransformPoint(localForward) - targetGameObject.transform.position;
            //Debug.Log("ForceObjectBarrier: Target hit right side. Moving target further to the right.");
            //Debug.Log("ForceObjectBarrier: Applying force vector " + forceVector + ". Current object vector " + targetGameObject.transform.position);
            targetGameObject.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        }
    }

    void ResetObjectPosition(int instanceId)
    {
        //Event feuern
        //UnityEngine.Debug.Log("ForceObjectBarrier: Reset instance " + instanceId);
        EventsManager.instance.OnResetObject(instanceId);
    }
}
