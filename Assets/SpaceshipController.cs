using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SpaceshipController : MonoBehaviour
{ 

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
    // Start is called before the first frame update
    void Start()
    {
  
        director  = this.GetComponent<PlayableDirector>();

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



    // Update is called once per frame
    void Update()
    {
   


    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GELANDET");
    }
}
