using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ShipDepartureTrigger : MonoBehaviour
{
    public GameObject timeLine = null;
    private PlayableDirector myDirector = null;
    private GameObject player = null;
    private bool shouldDisablePlayerMovement = false;
    private float playerEnteredTriggerTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        myDirector = timeLine.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;
        player = GameObject.FindWithTag("Player");
        Debug.Log("Hello from " + gameObject.GetInstanceID() + ", " + gameObject.GetComponent<ShipDepartureTrigger>().enabled);
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
            shouldDisablePlayerMovement = true;
        }
        
        
        
        
        //close door
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        //load credits scene
    }
}
