using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SpaceshipController : MonoBehaviour
{

    public GameObject door;
    private Animator doorAnimator;
    private List<GameObject> ShieldList = new List<GameObject>();
    private const string ShieldTag = "SpaceshipShield";
    private const string Bridgename = "Bridge";
    public const float SHIELD_Y_OFFSET = 2f;
    private PlayableDirector director;
    private SphereCollider SpaceshipCollider;
    enum SpaceshipStates {
        FLYING_NORMAL, FYLING_DANGER,LANDING,LANDED

    }

    private int SpaceshipState = (int) SpaceshipStates.FLYING_NORMAL;
    private bool HasDoorOpened = false;
    // Start is called before the first frame update
    void Start()
    {

        this.doorAnimator = this.door.GetComponent<Animator>();
      //  director  = this.GetComponent<PlayableDirector>();

        this.SpaceshipCollider = this.GetComponent<SphereCollider>();


        Transform bridge = this.gameObject.transform.Find(Bridgename);
        foreach (Transform child in bridge)
        {
            if (child.tag == ShieldTag)
                child.gameObject.transform.position = new Vector3(child.gameObject.transform.position.x, child.gameObject.transform.position.y + SHIELD_Y_OFFSET, child.gameObject.transform.position.z);
               ShieldList.Add(child.gameObject);
        }




    }


    private void CheckingLanding()
    {


        for (int i = 0; i < this.director.playableGraph.GetOutputCount(); i++)
        {
            Playable playable = this.director.playableGraph.GetOutput(i).GetSourcePlayable();
        }
    }

    private void OpenDoorsAfterLanding()
    {

        if(!HasDoorOpened)
        {

            this.doorAnimator.SetBool("character_nearby", true);

            HasDoorOpened = true;

        }

    }



    // Update is called once per frame
    void Update()
    {
   
     switch((int) SpaceshipState) {


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
     if(collision.gameObject.name.Equals("UpperPart"))
        {
            this.SpaceshipState = (int)SpaceshipStates.LANDED;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;

            Destroy(this.gameObject.GetComponent<Rigidbody>());

        }
    }
}
