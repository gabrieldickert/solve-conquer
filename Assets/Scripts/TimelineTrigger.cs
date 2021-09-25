using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public bool playOnlyOnce = true;
    public GameObject timeLine = null;
    public GameObject nextTimelineTrigger = null;

    private PlayableDirector myDirector = null;
    private bool timeLinePlaying = false;
    public int TimelineID;
    public bool activateNextTriggerImmediately = false;
    public bool shouldResumeMusic = false;
    public bool shouldPauseMusic = false;

    // Start is called before the first frame update
    void Start()
    {
        myDirector = timeLine.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;
        if(shouldResumeMusic)
        {
            EventsManager.instance.OnAudioManagerPlay();
        }
        if(shouldPauseMusic)
        {
            EventsManager.instance.OnAudioManagerPause();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if(!timeLinePlaying && other.tag == "Player")
        {
            timeLinePlaying = true;
            EventsManager.instance.OnAddTimelineToQueue(TimelineID);
            if(nextTimelineTrigger != null && activateNextTriggerImmediately)
            {
                nextTimelineTrigger.SetActive(true);
            }
            //Debug.Log("TimelineTrigger: Player entered trigger");
        }
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        if (playOnlyOnce && aDirector == myDirector)
        {
            gameObject.GetComponent<TimelineTrigger>().enabled = false;
        } else
        {
            timeLinePlaying = false;
        }

        if(nextTimelineTrigger != null && !activateNextTriggerImmediately)
        {
            nextTimelineTrigger.SetActive(true);
        }
            
    }
}
