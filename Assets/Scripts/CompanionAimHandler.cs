using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class CompanionAimHandler : MonoBehaviour
{

    public LayerMask AimCollisionLayerMask;

    //render our hand pointer raycast
    public LineRenderer telekinesisLine;

    //information about the line render
    public float lineWidth = 0.01f;
    public float lineMaxLength = 1f;

    //bool to determine if the line reder is enabled or disabled
    public bool toggled = false;

    
    private bool leftIndexTriggerDown = false;
    private bool leftIndexTriggerUp = false;
    private bool buttonYDown = false;
    private bool buttonXDown = false;
    private Vector3 companionWaitingPosition;

    
    

    //gameobject to store the enemy
    private GameObject enemy;

    private GameObject objectHit;

    // Start is called before the first frame update
    void Start()
    {
        //set up our line rederer
        Vector3[] startLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        telekinesisLine.SetPositions(startLinePositions);
        telekinesisLine.enabled = false;
        telekinesisLine.startWidth = lineWidth;
        telekinesisLine.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        
        leftIndexTriggerDown = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger);
        leftIndexTriggerUp = OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger);
        buttonYDown = OVRInput.GetDown(OVRInput.RawButton.Y);
        buttonXDown = OVRInput.GetDown(OVRInput.RawButton.X);

        if (buttonYDown)
        {
            EventsManager.instance.OnCompanionFollow();
        }

        if (buttonXDown)
        {
            EventsManager.instance.OnCompanionDropObject();
        }
        
        
        if (leftIndexTriggerDown)
        {
            toggled = true;
            telekinesisLine.enabled = true;
            Debug.Log("TelekinesisHand: leftIndexTriggerUp detected");
            
        } else if (leftIndexTriggerUp)
        {
            telekinesisLine.enabled = false;
            toggled = false;
            
            
            if (this.objectHit.tag == "GrabbableObject")
            {
                //pick up Grabbable
                EventsManager.instance.OnCompanionPickUpObject(this.objectHit);
            }
            else if (this.objectHit.tag == "HackableObject")
            {
                //hack object
                EventsManager.instance.OnCompanionHackObject(this.objectHit);
            }
            else
            {
                EventsManager.instance.OnCompanionWaitAt(this.companionWaitingPosition);
            }
        }

        if(toggled)
        {
            DetermineCompanionCommand(transform.position, transform.forward, lineMaxLength);
        }

    }

    private void DetermineCompanionCommand(Vector3 targetPosition, Vector3 direction, float length)
    {
        //set up raycast hit
        RaycastHit hit;

        //set up raycast
        Ray telekinesisOut = new Ray(targetPosition, direction);

        //declares an end position variable for the line renderer
        Vector3 endPosition = targetPosition + (length * direction);
        

        //run the raycast
        if (Physics.Raycast(telekinesisOut, out hit, lineMaxLength, AimCollisionLayerMask))
        {
            //update the line render with the new end position
            endPosition = hit.point;
            this.companionWaitingPosition = endPosition;

            //set the objectHit game object to the gameobject that the raycast hit
            objectHit = hit.collider.gameObject;

            
        } 
        telekinesisLine.SetPosition(0, gameObject.transform.InverseTransformPoint(targetPosition));
        telekinesisLine.SetPosition(1, gameObject.transform.InverseTransformPoint(endPosition));
    }
}
