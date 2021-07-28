using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipHandler : MonoBehaviour
{

    public GameObject LandingPlatform;

    public GameObject SpaceshipWindow;

    public float windowYOffset;

    private Vector3 FirstLandingPoint;

    private Vector3 SecondLandingPoint;


    private int YOffset = 5;

    private float FirstLandingPntDistance;

    private bool HasFirstLandingPos = false;

    private bool HasLanded = false;

    private bool IsRotating = false;

    

    // Start is called before the first frame update
    void Start()
    {


        this.FirstLandingPoint = new Vector3(this.LandingPlatform.transform.position.x, this.gameObject.transform.position.y, this.LandingPlatform.transform.position.z);
        this.SecondLandingPoint = new Vector3(this.LandingPlatform.transform.position.x, this.LandingPlatform.transform.position.y+YOffset, this.LandingPlatform.transform.position.z);



        this.FirstLandingPntDistance = Vector3.Distance(this.gameObject.transform.position, this.FirstLandingPoint);

      
        //this.SpaceshipWindow.transform.position = this.WindowTargetPnt;
    }


    void SpaceShipRotating()
    {
 
 
    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        float step = 10f * Time.deltaTime; // calculate distance to move
                                           // SpaceShipRotating();
        float rotationsPerMinute = 10.0f;
        //this.gameObject.transform.Rotate(Vector3.up * 4 * Time.deltaTime);

        Debug.Log(this.gameObject.transform.eulerAngles);


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
        }
  

    }
}
