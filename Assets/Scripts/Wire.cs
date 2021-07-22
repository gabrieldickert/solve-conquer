using System;
using UnityEngine;

public class Wire : MonoBehaviour
{
    Renderer WireRenderer;
    public int ActivatedByTriggerId;
    public bool isActiveOnStart = false;
    // Start is called before the first frame update
    void Start()
    {
        WireRenderer = GetComponent<Renderer>();
        WireRenderer.material.color = isActiveOnStart ? Color.green : Color.red;

        EventsManager.instance.PressurePlateEnable += ActivateWire;
        EventsManager.instance.PressurePlateDisable += DeactivateWire;
        EventsManager.instance.SwitchEnable += ActivateWire;
        EventsManager.instance.SwitchDisable += DeactivateWire;
        EventsManager.instance.HackableEnable += ActivateWire;
        EventsManager.instance.HackableDisable += DeactivateWire;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivateWire(int id)
    {
        if (id == ActivatedByTriggerId)
            WireRenderer.material.color = Color.green;
    }

    private void DeactivateWire(int id)
    {
        if (id == ActivatedByTriggerId)
            WireRenderer.material.color = Color.red;
    }
}