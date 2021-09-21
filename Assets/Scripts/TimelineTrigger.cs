using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public bool playOnlyOnce = true;
    public PlayableDirector timeLine = null;

    private bool timeLinePlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        timeLine.stopped += OnTimeLineStopped;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!timeLinePlaying && other.tag == "Player")
        {
            timeLinePlaying = true;
            timeLine.Play();
        }
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        if (playOnlyOnce && aDirector == timeLine)
        {
            gameObject.GetComponent<TimelineTrigger>().enabled = false;
        } else
        {
            timeLinePlaying = false;
        }

    }
}
