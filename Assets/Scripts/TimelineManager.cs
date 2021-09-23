using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
public class TimelineManager: MonoBehaviour
{
    public GameObject[] TimelineElements;
    public List<GameObject> TimelineQueue = new List<GameObject>();

    private bool IsPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < TimelineElements.Length;i++)
        {
            TimelineElements[i].GetComponent<PlayableDirector>().stopped += TimelineManager_stopped;
        }

        EventsManager.instance.AddTimelineToQueue += Instance_AddTimelineToQueue;
    }

    private void Instance_AddTimelineToQueue(int timelineID)
    {
        /*   Debug.Log("TIMELINE ID ADDEN" + timelineID);
           bool HasAlreadyInQueue = false;
           foreach (GameObject o in TimelineQueue) {

               for(int  i= 0; i < TimelineElements.Length;i++)
               {
                   if(o.GetInstanceID() == TimelineElements[i].GetInstanceID())
                   {
                       HasAlreadyInQueue = true;
                       break;
                   }
               }
           }
        */
        /*   if(!HasAlreadyInQueue)
           {
               //Index -1 due nature of array 
               Debug.Log("TimelineID:" + timelineID);
               this.TimelineQueue.Add(TimelineElements[timelineID - 1]);

               if(!IsPlaying)
               {
                   this.TimelineQueue.Last().GetComponent<PlayableDirector>().Play();
               }
           }*/
        this.TimelineQueue.Add(TimelineElements[timelineID - 1]);

        if (!IsPlaying)
        {
            this.TimelineQueue.Last().GetComponent<PlayableDirector>().Play();
            this.IsPlaying = true;
        }

    }

    private void TimelineManager_stopped(PlayableDirector obj)
    {

        this.TimelineQueue.Remove(obj.transform.gameObject);
        this.IsPlaying = false;

        if (this.TimelineQueue.Count > 0)
        {
            this.TimelineQueue.First().GetComponent<PlayableDirector>().Play();
            this.IsPlaying = true;
        }
    
     //   foreach(GameObject O in TimelineQueue)
       // {

      //  }
    }


    // Update is called once per frame
    void Update()
    {


        
    }
}
