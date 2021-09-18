using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger1 : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject previousTimeLine;
    public GameObject player = null;
    private float playerEnteredTriggerTime = 0;
    private bool playerEnteredTrigger = false;
    private bool nextTimelineStarted = false;


    /*void OnEnable()
    {
        previousTimeLine.GetComponent<PlayableDirector>().stopped += OnPlayableDirectorStopped;
    }*/

    private void Start()
    {
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        previousTimeLine.GetComponent<PlayableDirector>().stopped += OnPlayableDirectorStopped;
    }

    private void Update()
    {
        if(playerEnteredTrigger && Time.time - playerEnteredTriggerTime > 2)
        {
            player.GetComponent<Rigidbody>().isKinematic = true;
            if(!nextTimelineStarted)
            {
                timeLine.GetComponent<PlayableDirector>().Play();
                this.nextTimelineStarted = true;
            }
        } 
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (previousTimeLine.GetComponent<PlayableDirector>() == aDirector)
        {
            //Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
            
    }

    /*void OnDisable()
    {
        previousTimeLine.GetComponent<PlayableDirector>().stopped -= OnPlayableDirectorStopped;
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            this.player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
            //timeLine.GetComponent<PlayableDirector>().Play();
            previousTimeLine.GetComponent<PlayableDirector>().stopped -= OnPlayableDirectorStopped;
            this.playerEnteredTriggerTime = Time.time;
            this.playerEnteredTrigger = true;
            player.transform.Find("LocomotionController").GetComponent<TeleportInputHandlerTouch>().enabled = false;
            MeshRenderer triggerRenderer = gameObject.GetComponent<MeshRenderer>();
            //player.transform.position = new Vector3(triggerRenderer.bounds.center.x, player.transform.position.y, triggerRenderer.bounds.center.z);
        }
    }
}
