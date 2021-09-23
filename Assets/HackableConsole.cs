using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HackableConsole : MonoBehaviour
{
    public bool playOnlyOnce = true;
    public GameObject timeLine = null;
    public bool hasAdditionalTriggers = false;
    public int additionalTriggerId = 0;
    public GameObject timeLineTrigger = null;
    public GameObject additionalTimeline = null;
    public bool hasAdditionalTimeline = false;
    public GameObject alternativeTrigger = null;

    private PlayableDirector myDirector = null;
    private PlayableDirector additionalDirector = null;
    private bool timeLinePlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.CompanionHackEnable += HandleHacked;
        myDirector = timeLine.GetComponent<PlayableDirector>();
        myDirector.stopped += OnTimeLineStopped;
        if (hasAdditionalTimeline) {
            additionalDirector = additionalTimeline.GetComponent<PlayableDirector>();
        }
    }

    private void HandleHacked(int instanceId)
    {
        if(gameObject.GetInstanceID() == instanceId)
        {
            if (!timeLinePlaying)
            {
                timeLinePlaying = true;
                myDirector.Play();
                if(hasAdditionalTriggers)
                {
                    EventsManager.instance.OnSwitchDisable(additionalTriggerId);
                    timeLineTrigger.GetComponent<BoxCollider>().enabled = true;
                }
                if(hasAdditionalTimeline)
                {
                    additionalDirector.Play();
                }
                if(alternativeTrigger != null)
                {
                    alternativeTrigger.GetComponent<Ending1CutsceneTrigger>().enabled = false;
                }
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
