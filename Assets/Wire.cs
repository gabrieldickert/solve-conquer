using System;
using UnityEngine;

public class Wire : MonoBehaviour
{
    Renderer WireRenderer;
    public int ActivatedByTriggerId;
    // Start is called before the first frame update
    void Start()
    {
        WireRenderer = GetComponent<Renderer>();
        WireRenderer.material.color = Color.red;

        EventsManager.instance.PressurePlateEnable += ActivateWire;
        EventsManager.instance.PressurePlateDisable += DeactivateWire;
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