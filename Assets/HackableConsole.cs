using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HackableConsole : MonoBehaviour
{
    public bool playOnlyOnce = true;
    public GameObject timeLine = null;

    private PlayableDirector myDirector = null;
    private bool timeLinePlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.CompanionHackEnable += HandleHacked;
        myDirector = timeLine.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;
    }

    private void HandleHacked(int instanceId)
    {
        if(gameObject.GetInstanceID() == instanceId)
        {
            if (!timeLinePlaying)
            {
                timeLinePlaying = true;
                myDirector.Play();
            }

            if (playOnlyOnce)
            {
                gameObject.GetComponent<HackableConsole>().enabled = false;
            }
        }
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        if(!playOnlyOnce)
        {
            timeLinePlaying = false;
        }
    }
}
