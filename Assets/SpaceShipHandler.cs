using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipHandler : MonoBehaviour
{

    public GameObject LandingPlatform;

    public GameObject SpaceshipWindow;

    public GameObject SpaceshipDoor;

    public List<Vector3> FlyPathList;

    public int PntIndexToTriggerWindows;

    public int LandingPointYOffset = 5;

    public float windowYOffset;

    public float rotationSpeed = 3;

    public float rotationRange = 60;

    public float flyingspeed =50f;

    public float landingspeed = 10f;

    private int current;

  

    // Start is called before the first frame update
    void Start()
    {


        //    this.FirstLandingPoint = new Vector3(this.LandingPlatform.transform.position.x, this.gameObject.transform.position.y, this.LandingPlatform.transform.position.z);
        //this.SecondLandingPoint = new Vector3(this.LandingPlatform.transform.position.x, this.LandingPlatform.transform.position.y+YOffset, this.LandingPlatform.transform.position.z);


        //if Spaceship gets a Landing Plattform assigned an additional point gets added
        if(this.LandingPlatform != null) {

            FlyPathList.Add(new Vector3(this.LandingPlatform.transform.position.x, this.gameObject.transform.position.y, this.LandingPlatform.transform.position.z));
            FlyPathList.Add(new Vector3(this.LandingPlatform.transform.position.x, this.LandingPlatform.transform.position.y + LandingPointYOffset, this.LandingPlatform.transform.position.z));
        }


        //this.spaceshipStartPos = this.gameObject.transform.position;
    }


    // Update is called once per frame
    void Update()
    {

        float selectedspeed = (current < FlyPathList.Count - 1) ? this.flyingspeed : this.landingspeed;
        // Move our position a step closer to the target.
        float step = selectedspeed * Time.deltaTime; // calculate distance to move
                                           // SpaceShipRotating();

        //this.gameObject.transform.Rotate(Vector3.up * 4 * Time.deltaTime);

        if(this.gameObject.transform.position != FlyPathList[current])
        {


            if(current == PntIndexToTriggerWindows && this.SpaceshipWindow != null)
            {

                this.SpaceshipWindow.transform.position = Vector3.MoveTowards(this.SpaceshipWindow.transform.position, new Vector3(this.SpaceshipWindow.transform.position.x, this.SpaceshipWindow.transform.position.y + this.windowYOffset, this.SpaceshipWindow.transform.position.z), step);
            }
            //Checking if spaceship is landing
            if(current == FlyPathList.Count-2)
            {
                    
               
            }
            float AngleAmount = (Mathf.Cos(Time.time * rotationSpeed) * 180) / Mathf.PI * 0.5f;

            AngleAmount = Mathf.Clamp(AngleAmount, rotationRange *(-1) , rotationRange);

            this.gameObject.transform.rotation = Quaternion.Euler(0, AngleAmount, 0);

            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, FlyPathList[current], step);

        }


        else
        {

            //Only incrementing when current is smaller then size of pointlist
            if(current < FlyPathList.Count-1)
            {
                current++;
            }
      
        }



    }
}
