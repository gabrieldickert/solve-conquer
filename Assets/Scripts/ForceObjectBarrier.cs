using UnityEngine;
using OculusSampleFramework;
using UnityEngine.AI;

public class ForceObjectBarrier : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public AudioClip sound_objectBlocked;
    
    private AudioSource source;
    private OVRGrabber LeftHandGrabber;
    private OVRGrabber RightHandGrabber;

    void Start()
    {
        this.LeftHandGrabber = this.LeftHand.GetComponent<DistanceGrabber>();
        this.RightHandGrabber = this.RightHand.GetComponent<DistanceGrabber>();
        this.source = gameObject.AddComponent<AudioSource>();
        EventsManager.instance.ForceObjectBarrierEnableObstacle += HandleForceObjectBarrierEnableObstacle;
        EventsManager.instance.ForceObjectBarrierDisableObstacle += HandleForceObjectBarrierDisableObstacle;
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
                Reposition(other.gameObject);
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
            targetGameObject.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        }
        else if (targetDirectionLocal.z > 0)
        {
            Vector3 forceVector = transform.TransformPoint(localForward) - targetGameObject.transform.position;
            //Debug.Log("ForceObjectBarrier: Target hit right side. Moving target further to the right.");
            targetGameObject.GetComponent<Rigidbody>().AddForce(forceVector, ForceMode.Impulse);
        }
    }

    void HandleForceObjectBarrierEnableObstacle()
    {
        gameObject.transform.parent.GetComponent<NavMeshObstacle>().enabled = true;
    }

    void HandleForceObjectBarrierDisableObstacle()
    {
        gameObject.transform.parent.GetComponent<NavMeshObstacle>().enabled = false;
    }
}
