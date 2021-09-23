using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Ending1CutsceneTrigger : MonoBehaviour
{
    public int listenForTriggerId = 0;
    public int disableTriggerId = 0;
    public bool playOnlyOnce = true;
    public GameObject timeLine = null;
    public GameObject triggerShipFlight = null;
    public GameObject alternativeTrigger = null;
    public GameObject reactorTimeline = null;

    private PlayableDirector myDirector = null;
    private PlayableDirector reactorDirector = null;
    private bool timeLinePlaying = false;

    void Start()
    {
        EventsManager.instance.SwitchEnable += HandleSwitchEnable;
        myDirector = timeLine.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;

        reactorDirector = reactorTimeline.GetComponent<PlayableDirector>();
    }

    private void HandleSwitchEnable(int triggerId)
    {
        if(this.listenForTriggerId == triggerId)
        {
            if (!timeLinePlaying)
            {
                timeLinePlaying = true;
                myDirector.Play();
                reactorDirector.Play();
                EventsManager.instance.OnSwitchDisable(disableTriggerId);
                this.triggerShipFlight.GetComponent<BoxCollider>().enabled = true;
                GameObject.FindWithTag("Companion").GetComponent<NavMeshAgent>().enabled = false;
            }

            if (playOnlyOnce)
            {
                if(alternativeTrigger != null)
                {
                    alternativeTrigger.GetComponent<HackableConsole>().enabled = false;
                }
            }
        }
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        if (!playOnlyOnce)
        {
            timeLinePlaying = false;
        } else
        {
            StartCoroutine(LoadCreditsSceneAsync());
            gameObject.GetComponent<Ending1CutsceneTrigger>().enabled = false;
        }
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
