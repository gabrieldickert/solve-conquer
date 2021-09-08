using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpaceShipLandingSceneScript : MonoBehaviour
{

    private PlayableDirector pd;
    private GameObject Player;
    public GameObject door;
    private Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        pd = this.GetComponent<PlayableDirector>();
        pd.stopped += OnPlayableDirectorStopped;
        Player = GameObject.FindWithTag("Player");
        Player.GetComponent<Rigidbody>().isKinematic = true;
        Player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
        this.doorAnimator = this.door.GetComponent<Animator>();
        
        
    }

    // Update is called once per frame
    void Update()
    {


    }
    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {

        Player.GetComponent<Rigidbody>().isKinematic = false;
        Player.transform.parent = null;
        this.doorAnimator.SetBool("character_nearby", true);
        Player.transform.Find("LocomotionController").GetComponent<LocomotionController>().enabled = true;
        transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").gameObject.GetComponent<LineRenderer>().enabled = true;
        transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").gameObject.GetComponent<CompanionAimHandler>().enabled = true;
        Player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
    }

}
