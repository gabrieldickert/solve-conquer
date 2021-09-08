using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger1 : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject previousTimeLine;
    public GameObject player = null;
    private float playerEnteredTriggerTime = 0;
    private bool playerEnteredTrigger = false;


    /*void OnEnable()
    {
        previousTimeLine.GetComponent<PlayableDirector>().stopped += OnPlayableDirectorStopped;
    }*/

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        previousTimeLine.GetComponent<PlayableDirector>().stopped += OnPlayableDirectorStopped;
        player.transform.Find("LocomotionController").GetComponent<LocomotionController>().enabled = false;
    }

    private void Update()
    {
        if(playerEnteredTrigger && Time.time - playerEnteredTriggerTime > 2)
        {
            player.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (previousTimeLine.GetComponent<PlayableDirector>() == aDirector)
        {
            Debug.Log("PlayableDirector named " + aDirector.name + " is now stopped.");
            gameObject.GetComponent<BoxCollider>().enabled = true;
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
            timeLine.GetComponent<PlayableDirector>().Play();
            previousTimeLine.GetComponent<PlayableDirector>().stopped -= OnPlayableDirectorStopped;
            this.playerEnteredTriggerTime = Time.time;
            this.playerEnteredTrigger = true;
        }
    }
}
