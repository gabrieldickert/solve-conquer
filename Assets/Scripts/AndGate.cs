using System.Collections.Generic;
using UnityEngine;

public class AndGate : MonoBehaviour
{
    public List<int> activatedByTriggerId = new List<int>();
    public bool isActiveOnStart = true;
    public int triggerId = 0;
    private List<int> activeTriggers = new List<int>();
    private Renderer gateRenderer;

    void Start()
    {
        EventsManager.instance.PressurePlateEnable += HandleActivatorEnable;
        EventsManager.instance.PressurePlateDisable += HandleActivatorDisable;
        EventsManager.instance.SwitchEnable += HandleActivatorEnable;
        EventsManager.instance.SwitchDisable += HandleActivatorDisable;
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
                EventsManager.instance.OnLogicGateEnable(this.triggerId);
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
                EventsManager.instance.OnLogicGateDisable(this.triggerId);
                gateRenderer.material.color = Color.red;
            }
        }
    }
}
