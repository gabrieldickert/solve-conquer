using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{ 

    private List<GameObject> ShieldList = new List<GameObject>();
    private const string ShieldTag = "SpaceshipShield";
    private const string Bridgename = "Bridge";
    public const float SHIELD_Y_OFFSET = 2f;
    enum SpaceshipStates {
        FLYING_NORMAL, FYLING_DANGER,LANDING,LANDED

    }

    private int SpaceshipState = (int) SpaceshipStates.FLYING_NORMAL;
    // Start is called before the first frame update
    void Start()
    {

        Transform bridge = this.gameObject.transform.Find(Bridgename);
        foreach (Transform child in bridge)
        {
            if (child.tag == ShieldTag)
                child.gameObject.transform.position = new Vector3(child.gameObject.transform.position.x, child.gameObject.transform.position.y + SHIELD_Y_OFFSET, child.gameObject.transform.position.z);
               ShieldList.Add(child.gameObject);
        }




    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
