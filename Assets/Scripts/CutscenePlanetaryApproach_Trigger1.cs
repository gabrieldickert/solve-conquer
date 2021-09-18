using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutscenePlanetaryApproach_Trigger1 : MonoBehaviour
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
        timeLine.GetComponent<PlayableDirector>().stopped += OnLoadPlanetScene;
    }

    private void Update()
    {
        if(playerEnteredTrigger && Time.time - playerEnteredTriggerTime > 2)
        {
            if(!nextTimelineStarted)
            {
                player.transform.Find("LocomotionController").gameObject.SetActive(false);
                player.GetComponent<Rigidbody>().isKinematic = true;
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

    void OnLoadPlanetScene(PlayableDirector aDirector)
    {
        //Debug.Log("ALERT: TIMELINE STOPPED: LOAD PLANETSCENE");
        StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlanetScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
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
            
            previousTimeLine.GetComponent<PlayableDirector>().stopped -= OnPlayableDirectorStopped;
            this.playerEnteredTriggerTime = Time.time;
            this.playerEnteredTrigger = true;
            
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
