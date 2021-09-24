using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTriggerVisual : MonoBehaviour
{
    public GameObject timeLine = null;
    private PlayableDirector myDirector = null;

    // Start is called before the first frame update
    void Start()
    {
        if (timeLine != null)
        {
            myDirector = timeLine.GetComponent<PlayableDirector>();
            myDirector.stopped += OnTimeLineStopped;
        } else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    void OnTimeLineStopped(PlayableDirector aDirector)
    {
        if (aDirector == myDirector)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
