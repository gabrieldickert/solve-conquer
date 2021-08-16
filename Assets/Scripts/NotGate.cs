using System.Collections.Generic;
using UnityEngine;

public class NotGate : MonoBehaviour
{
    public int activatedByTriggerId = 0;
    public bool isActiveOnStart = false;
    public int triggerId = 0;
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
        if (activatedByTriggerId == triggerId)
        {
            //close barrier/ bridge
            EventsManager.instance.OnLogicGateDisable(this.triggerId);
            gateRenderer.material.color = Color.red;
        }
    }

    void HandleActivatorDisable(int triggerId)
    {

        if (activatedByTriggerId == triggerId)
        {
            //open barrier/ bridge
            EventsManager.instance.OnLogicGateEnable(this.triggerId);
            gateRenderer.material.color = Color.green;
        }
    }
}
