using System.Collections.Generic;
using UnityEngine;

public class AndGate : MonoBehaviour
{
    public List<int> activatedByTriggerId = new List<int>();
    public bool isActiveOnStart = true;
    public int triggerId1 = 0;
    public int triggerId2 = 0;
    private List<int> activeTriggers = new List<int>();
    private Renderer gateRenderer;

    void Start()
    {
        EventsManager.instance.PressurePlateEnable += HandleActivatorEnable;
        EventsManager.instance.PressurePlateDisable += HandleActivatorDisable;
        EventsManager.instance.SwitchEnable += HandleActivatorEnable;
        EventsManager.instance.SwitchDisable += HandleActivatorDisable;
        EventsManager.instance.LogicGateEnable += HandleActivatorEnable;
        EventsManager.instance.LogicGateDisable += HandleActivatorDisable;
        
        gateRenderer = GetComponent<Renderer>();
        gateRenderer.material.color = isActiveOnStart ? Color.green : Color.red;
    }

   
    void HandleActivatorEnable(int triggerId)
    {
        if (activatedByTriggerId.Contains(triggerId))
        {
            if (!activeTriggers.Contains(triggerId))
            {
                activeTriggers.Add(triggerId);
            }
            if (activeTriggers.Count == activatedByTriggerId.Count)
            {
                //open barrier/ bridge
                EventsManager.instance.OnLogicGateEnable(this.triggerId1);
                EventsManager.instance.OnLogicGateEnable(this.triggerId2);
                gateRenderer.material.color = Color.green;
            }
        }
    }

    void HandleActivatorDisable(int triggerId)
    {
       
        if (activatedByTriggerId.Contains(triggerId))
        {
            if (activeTriggers.Contains(triggerId))
            {
                activeTriggers.Remove(triggerId);
            }
            if ((activeTriggers.Count != activatedByTriggerId.Count))
            {
                //close barrier/ bridge
                EventsManager.instance.OnLogicGateDisable(this.triggerId1);
                EventsManager.instance.OnLogicGateDisable(this.triggerId2);
                gateRenderer.material.color = Color.red;
            }
        }
    }
}
