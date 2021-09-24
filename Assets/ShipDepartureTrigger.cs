using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ShipDepartureTrigger : MonoBehaviour
{
    public GameObject prevTimeline = null;
    public GameObject timeLine = null;
    public GameObject reactorTimeline = null;

    private PlayableDirector myDirector = null;
    private PlayableDirector prevDirector = null;
    private PlayableDirector reactorDirector = null;
    private GameObject player = null;
    private bool shouldDisablePlayerMovement = false;
    private float playerEnteredTriggerTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        myDirector = timeLine.GetComponent<PlayableDirector>();
        prevDirector = prevTimeline.GetComponent<PlayableDirector>();
        reactorDirector = reactorTimeline.GetComponent<PlayableDirector>();
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
            reactorDirector.Stop();
            shouldDisablePlayerMovement = true;
        }
        
        
        
        
        //close door
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        //load credits scene
        StartCoroutine(LoadCreditsSceneAsync());
    }

    IEnumerator LoadCreditsSceneAsync()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CreditScene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
