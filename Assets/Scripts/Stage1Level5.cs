using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Level5 : MonoBehaviour
{
    public int triggerIdCompanionDrop = 0;
    public int triggerFloor2 = 0;
    public int triggerFloor3 = 0;
    public int triggerFloor4 = 0;
    public int triggerFloor5 = 0;

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

    

    private void HandleDropCompanion(int triggerId)
    {
        if(triggerIdCompanionDrop == triggerId && !companionDropped)
        {
            companionDropped = true;
            Debug.Log("Stage1Level5: Dropping Companion");


        }
    }

    
    private void HandlePlayerEnteredTrigger(int triggerId)
    {
        if(triggerId == triggerFloor2 && currentFloor != 2)
        {
            Debug.Log("Stage1Level5: Player entered floor 2");
            currentFloor = 2;
        } else if(triggerId == triggerFloor3 && currentFloor != 3)
        {
            Debug.Log("Stage1Level5: Player entered floor 3");
            currentFloor = 3;
        }
        else if (triggerId == triggerFloor4 && currentFloor != 4)
        {
            Debug.Log("Stage1Level5: Player entered floor 4");
            currentFloor = 4;
        }
        else if (triggerId == triggerFloor5 && currentFloor != 5)
        {
            Debug.Log("Stage1Level5: Player entered floor 5");
            currentFloor = 5;
        }
    }
}
