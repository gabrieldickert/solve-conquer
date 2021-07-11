using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
public class ObjectBarrier : MonoBehaviour
{



    public GameObject LeftHand;
    public GameObject RightHand;
    


    private OVRGrabber LeftHandGrabber;
    private OVRGrabber RightHandGrabber;




    // Start is called before the first frame update
    void Start()
    {

      this.LeftHandGrabber = this.LeftHand.GetComponent<DistanceGrabber>();
      this.RightHandGrabber = this.RightHand.GetComponent<DistanceGrabber>();
    //Make the ObjectBarrier Red so it cannot be passed with Objects in Hands
      gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 0.1f);
      


    }

    // Update is called once per frame
    void Update()
    {


    }


    private void OnTriggerEnter(Collider other)
    {


         //Ensure dropping items when player passes barrier
        if(other.gameObject.name.Equals("PlayerController"))
        {

               if (this.LeftHandGrabber.grabbedObject != null)
                {

                    this.LeftHandGrabber.ForceRelease(this.LeftHandGrabber.grabbedObject);

                }

                if (this.RightHandGrabber.grabbedObject != null)
                {


                    this.RightHandGrabber.ForceRelease(this.RightHandGrabber.grabbedObject);
                }

 

        }




    }

    private void OnTriggerExit(Collider other)
    {

 


       

    }






}
