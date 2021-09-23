using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShipDepartureTrigger : MonoBehaviour
{
    public GameObject prevTimeline = null;
    public GameObject timeLine = null;
    private PlayableDirector myDirector = null;
    private PlayableDirector prevDirector = null;
    private GameObject player = null;
    private bool shouldDisablePlayerMovement = false;
    private float playerEnteredTriggerTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        myDirector = timeLine.GetComponent<PlayableDirector>();
        prevDirector = prevTimeline.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if(shouldDisablePlayerMovement && Time.time - playerEnteredTriggerTime > 2f)
        {
            player.transform.Find("LocomotionController").gameObject.SetActive(false);
            player.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<ShipDepartureTrigger>().enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!shouldDisablePlayerMovement)
        {
            playerEnteredTriggerTime = Time.time;
            myDirector.Play();
            prevDirector.Stop();
            shouldDisablePlayerMovement = true;
        }
        
        
        
        
        //close door
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        //load credits scene
    }
}
