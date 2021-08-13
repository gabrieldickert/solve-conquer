using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleManager : MonoBehaviour
{
    public GameObject door;
    public float SHIELD_Y_OFFSET = 2f;
    public float ShieldOpeningWaitime = 5;
    public float WaitForDoorOpeningAfterLanding = 3f;
    private Animator doorAnimator;
    private List<GameObject> ShieldList = new List<GameObject>();
    private const string ShieldTag = "SpaceshipShield";
    private const string Bridgename = "Bridge";
    private SphereCollider SpaceshipCollider;
    private GameObject PlayerController;
    private enum SpaceshipStates
    {
        FLYING_NORMAL, FYLING_DANGER, LANDING, LANDED
    }

    private int SpaceshipState = (int)SpaceshipStates.FLYING_NORMAL;
    private bool HasDoorOpened = false;
    // Start is called before the first frame update
    void Start()
    {
        this.doorAnimator = this.door.GetComponent<Animator>();
        this.SpaceshipCollider = this.GetComponent<SphereCollider>();
        this.PlayerController = this.gameObject.transform.Find("PlayerController").gameObject;
        StartCoroutine(waiter());
    }
    private IEnumerator waiter()
    {
        //Wait
        yield return new WaitForSeconds(ShieldOpeningWaitime);

        Transform bridge = this.gameObject.transform.Find(Bridgename);
        foreach (Transform child in bridge)
        {
            if (child.tag == ShieldTag)
                child.gameObject.transform.position = new Vector3(child.gameObject.transform.position.x, child.gameObject.transform.position.y + SHIELD_Y_OFFSET, child.gameObject.transform.position.z);
            ShieldList.Add(child.gameObject);
        }
    }

    private IEnumerator WaitforDoorOpening()
    {


        yield return new WaitForSeconds(WaitForDoorOpeningAfterLanding);

        this.SpaceshipState = (int)SpaceshipStates.LANDED;
    }
    private void OpenDoorsAfterLanding()
    {
        if (!HasDoorOpened)
        {
            this.doorAnimator.SetBool("character_nearby", true);
            HasDoorOpened = true;
            this.PlayerController.transform.SetParent(null);
            this.PlayerController.GetComponent<Rigidbody>().isKinematic = false;
            this.PlayerController.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            //Remove Spaceship out of Local System of the Spaceship
            // this.PlayerController.transform.SetParent(this.Respawnables.transform);
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
        //Spaceship collided with Landing Plat
        if (collision.gameObject.name.Equals("LandingArea"))
        {

            Debug.Log("Collision with Landing Area");
            this.SpaceshipCollider.enabled = false;
           Destroy(collision.gameObject.GetComponent<Rigidbody>());
            StartCoroutine(WaitforDoorOpening());
        }
    }
}
