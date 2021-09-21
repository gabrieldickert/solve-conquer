using OculusSampleFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Stage1Level5 : MonoBehaviour
{
    public int triggerIdCompanionDrop = 0;
    public int triggerFloor2 = 0;
    public int triggerFloor3 = 0;
    public int triggerFloor4 = 0;
    public int triggerFloor5 = 0;
    public int[] bridgeFloor1;
    //public int[] bridgeFloor1_b;
    public int[] bridgeFloor2;
    public int[] bridgeFloor3;
    public int[] bridgeFloor4;
    public int bridgeFloor5 = 0;

    public GameObject companion = null;
    public PlayableDirector director = null;
    public PrefabSpawner trashSpawner = null;
    public GameObject cubeFloor2_a = null;
    public GameObject cubeFloor2_b = null;
    public GameObject cubeFloor3 = null;
    public GameObject cubeFloor4 = null;

    private int currentFloor = 0;
    bool companionDropped = false;
    

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.SwitchEnable += HandleDropCompanion;
        EventsManager.instance.PlayerEnteredTrigger += HandlePlayerEnteredTrigger;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (director == aDirector)
        {
            companion.GetComponent<NavMeshAgent>().enabled = true;
        }

    }



    private void HandleDropCompanion(int triggerId)
    {
        if(triggerIdCompanionDrop == triggerId && !companionDropped)
        {
            companionDropped = true;
            Debug.Log("Stage1Level5: Dropping Companion");
            director.Play();
            director.stopped += OnPlayableDirectorStopped;
        }
    }

    private void DropCube(GameObject cube)
    {
        cube.GetComponent<MeshRenderer>().enabled = true;
        cube.GetComponent<BoxCollider>().enabled = true;
        cube.GetComponent<Rigidbody>().isKinematic = false;
        cube.GetComponent<DistanceGrabbable>().enabled = true;
        //Debug.Log("CubeStatus: " + cube.GetComponent<Rigidbody>().isKinematic);
    }

    
    private void HandlePlayerEnteredTrigger(int triggerId)
    {
        if(triggerId == triggerFloor2 && currentFloor != 2)
        {
            Debug.Log("Stage1Level5: Player entered floor 2");
            currentFloor = 2;
            foreach(int bridgeTrigger in bridgeFloor1)
            {
                EventsManager.instance.OnPressurePlateDisable(bridgeTrigger);
            }
            /*foreach (int bridgeTrigger in bridgeFloor1_b)
            {
                EventsManager.instance.OnPressurePlateDisable(bridgeTrigger);
            }*/
            DropCube(cubeFloor2_a);
            DropCube(cubeFloor2_b);
        } else if(triggerId == triggerFloor3 && currentFloor != 3)
        {
            Debug.Log("Stage1Level5: Player entered floor 3");
            currentFloor = 3;
            foreach (int bridgeTrigger in bridgeFloor2)
            {
                EventsManager.instance.OnPressurePlateDisable(bridgeTrigger);
            }
            DropCube(cubeFloor3);
        }
        else if (triggerId == triggerFloor4 && currentFloor != 4)
        {
            Debug.Log("Stage1Level5: Player entered floor 4");
            currentFloor = 4;
            foreach (int bridgeTrigger in bridgeFloor3)
            {
                EventsManager.instance.OnPressurePlateDisable(bridgeTrigger);
            }
            DropCube(cubeFloor4);
        }
        else if (triggerId == triggerFloor5 && currentFloor != 5)
        {
            Debug.Log("Stage1Level5: Player entered floor 5");
            currentFloor = 5;
            trashSpawner.enabled = false;
            foreach (int bridgeTrigger in bridgeFloor4)
            {
                EventsManager.instance.OnPressurePlateDisable(bridgeTrigger);
            }
            EventsManager.instance.OnPressurePlateEnable(bridgeFloor5);
        }
    }
}
