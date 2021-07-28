using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipHandler : MonoBehaviour
{

    public GameObject LandingPlatform;

    public GameObject SpaceshipWindow;

    public float windowYOffset;

    public List<Vector3> FlyPathList;

    public float speed =50f;
    public float landingspeed = 10f;

    private int current;

    public int  LandingPointYOffset = 5;

    private float FirstLandingPntDistance;

    private bool HasFirstLandingPos = false;

    private bool HasLanded = false;

    private bool IsRotating = false;

    private Vector3 spaceshipStartPos;

    

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


        this.spaceshipStartPos = this.gameObject.transform.position;
    }


    void SpaceShipRotating()
    {
 
 
    }

    // Update is called once per frame
    void Update()
    {

        float selectedspeed = (current < FlyPathList.Count - 1) ? this.speed : this.landingspeed;
        // Move our position a step closer to the target.
        float step = selectedspeed * Time.deltaTime; // calculate distance to move
                                           // SpaceShipRotating();
        float rotationsPerMinute = 22.0f;
        //this.gameObject.transform.Rotate(Vector3.up * 4 * Time.deltaTime);

        if(this.gameObject.transform.position != FlyPathList[current])
        {
            float AngleAmount = (Mathf.Cos(Time.time * 3) * 180) / Mathf.PI * 0.5f;
            Debug.Log("Rotation " + AngleAmount);
            AngleAmount = Mathf.Clamp(AngleAmount, -60,60);

            this.gameObject.transform.rotation = Quaternion.Euler(0, AngleAmount, 0);
            // material.transform.localRotation = Quaternion.Euler(0, 0, AngleAmount);

            // this.gameObject.transform.Rotate(0, 10.0f * rotationsPerMinute * Time.deltaTime * 2, 0);
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






        //  Debug.Log(this.gameObject.transform.eulerAngles);




        /*
        if(this.gameObject.transform.eulerAngles.y < 180)
        {
           // this.gameObject.transform.Rotate(0, 10.0f * rotationsPerMinute * Time.deltaTime * 2, 0);

        }

        else if (this.gameObject.transform.eulerAngles.y > 180)
        {
           // this.gameObject.transform.Rotate(0, 10.0f * rotationsPerMinute * Time.deltaTime * 2 *-1, 0);
        }

        if(this.gameObject.transform.eulerAngles.y <= 0)
        {
                  //   this.gameObject.transform.Rotate(0, 10.0f * rotationsPerMinute * Time.deltaTime * 2, 0);

        }
        if (!HasFirstLandingPos && !HasLanded) {

            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, this.FirstLandingPoint, step);

            if (Vector3.Distance(this.gameObject.transform.position, this.FirstLandingPoint) < 0.001f)
            {

                HasFirstLandingPos = true;

            }
     

            else if(Vector3.Distance(this.gameObject.transform.position, this.FirstLandingPoint) < this.FirstLandingPntDistance/2)
            {
                Debug.Log("Jetzt window nach oben");

               this.SpaceshipWindow.transform.position = Vector3.MoveTowards(this.SpaceshipWindow.transform.position, new Vector3(this.SpaceshipWindow.transform.position.x, this.SpaceshipWindow.transform.position.y+this.windowYOffset, this.SpaceshipWindow.transform.position.z), step);

            }
        }

        else if (HasFirstLandingPos && !HasLanded)
        {


            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, this.SecondLandingPoint, step);

            if (Vector3.Distance(this.gameObject.transform.position,this.SecondLandingPoint) < 0.001f)
            {

                HasLanded = true;

            }
        }*/


    }
}
