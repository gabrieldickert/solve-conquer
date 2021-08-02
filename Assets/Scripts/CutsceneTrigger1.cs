using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger1 : MonoBehaviour
{
    public GameObject timeLine;
    public GameObject previousTimeLine;

    /*void OnEnable()
    {
        previousTimeLine.GetComponent<PlayableDirector>().stopped += OnPlayableDirectorStopped;
    }*/

    private void Start()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        previousTimeLine.GetComponent<PlayableDirector>().stopped += OnPlayableDirectorStopped;
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
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            timeLine.GetComponent<PlayableDirector>().Play();
            previousTimeLine.GetComponent<PlayableDirector>().stopped -= OnPlayableDirectorStopped;
        }
    }
}
