using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Ending1CutsceneTrigger : MonoBehaviour
{
    public int listenForTriggerId = 0;
    public int disableTriggerId = 0;
    public bool playOnlyOnce = true;
    public GameObject timeLine = null;

    private PlayableDirector myDirector = null;
    private bool timeLinePlaying = false;

    void Start()
    {
        EventsManager.instance.SwitchEnable += HandleSwitchEnable;
        myDirector = timeLine.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;
    }

    private void HandleSwitchEnable(int triggerId)
    {
        if(this.listenForTriggerId == triggerId)
        {
            if (!timeLinePlaying)
            {
                timeLinePlaying = true;
                myDirector.Play();
                EventsManager.instance.OnSwitchDisable(disableTriggerId);
            }

            if (playOnlyOnce)
            {
                gameObject.GetComponent<Ending1CutsceneTrigger>().enabled = false;
            }
        }
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        if (!playOnlyOnce)
        {
            timeLinePlaying = false;
        }
    }
}
