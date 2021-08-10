using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public GameObject door;
    private Animator doorAnimator;
    private List<GameObject> ShieldList = new List<GameObject>();
    private const string ShieldTag = "SpaceshipShield";
    private const string Bridgename = "Bridge";
    public  float SHIELD_Y_OFFSET = 2f;
    public float ShieldOpeningWaitime = 5; 
    private SphereCollider SpaceshipCollider;
    private GameObject PlayerController;

    enum SpaceshipStates
    {
        FLYING_NORMAL, FYLING_DANGER, LANDING, LANDED

    }

    private int SpaceshipState = (int)SpaceshipStates.FLYING_NORMAL;
    private bool HasDoorOpened = false;

    // Start is called before the first frame update
    void Start()
    {
        this.doorAnimator = this.door.GetComponent<Animator>();
        //  director  = this.GetComponent<PlayableDirector>();

        this.SpaceshipCollider = this.GetComponent<SphereCollider>();
        Transform bridge = this.gameObject.transform.Find(Bridgename);
        foreach(Transform child in  bridge)
        {

            if(child.tag == "Player")
            {

                this.PlayerController = child.gameObject;
                break;
            }

        }



        StartCoroutine(waiter());
     
    }
    IEnumerator waiter()
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(ShieldOpeningWaitime);

        Transform bridge = this.gameObject.transform.Find(Bridgename);
        foreach (Transform child in bridge)
        {
            if (child.tag == ShieldTag)
                child.gameObject.transform.position = new Vector3(child.gameObject.transform.position.x, child.gameObject.transform.position.y + SHIELD_Y_OFFSET, child.gameObject.transform.position.z);
            ShieldList.Add(child.gameObject);
        }

    }
    private void OpenDoorsAfterLanding()
    {

        if (!HasDoorOpened)
        {

            this.doorAnimator.SetBool("character_nearby", true);

            HasDoorOpened = true;

            //this.PlayerController.transform.SetParent(null);

        }

    }

    // Update is called once per frame
    void Update()
    {
        switch ((int)SpaceshipState)
        {


            case (int)SpaceshipStates.LANDED:
                OpenDoorsAfterLanding();
                break;


            default:
                break;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log("COLLISION"+collision.gameObject.name);

        //Spaceship collided with Landing Plat
        if (collision.gameObject.name.Equals("UpperPart"))
        {
            this.SpaceshipState = (int)SpaceshipStates.LANDED;

         // this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

           // Destroy(this.gameObject.GetComponent<Rigidbody>());

        }
    }
}
