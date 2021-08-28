using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MonoBehaviour
{
    public GameObject targetAntenna;
    public float rotationSpeed = 2f;
    public float lineWidth = 0.1f;
    public int triggerId1 = 0;
    public int triggerId2 = 0;
    public List<int> activatedByTriggerId = new List<int>();

    private Transform sphere;
    private Transform antenna;
    private Vector3 offset = Vector3.zero;
    private Transform targetTransform;
    private Transform targetTransformAntenna;
    private LineRenderer lineRenderer;
    private bool isEnabled = false;
    
    

    void Start()
    {
        EventsManager.instance.PressurePlateEnable += HandleAntennaEnabled;
        EventsManager.instance.LogicGateEnable += HandleAntennaEnabled;
        EventsManager.instance.SwitchEnable += HandleAntennaEnabled;
        EventsManager.instance.ThrowableTargetEnable += HandleAntennaEnabled;
        EventsManager.instance.AntennaEnable += HandleAntennaEnabled;

        EventsManager.instance.PressurePlateDisable += HandleAntennaDisabled;
        EventsManager.instance.LogicGateDisable += HandleAntennaDisabled;
        EventsManager.instance.SwitchDisable += HandleAntennaDisabled;
        EventsManager.instance.ThrowableTargetDisable += HandleAntennaDisabled;
        EventsManager.instance.AntennaDisable += HandleAntennaDisabled;

        antenna = transform.GetChild(0).GetChild(0).transform;
        sphere = transform.GetChild(0).transform;
        Debug.Log(sphere);
        Debug.Log(antenna);

        
        offset = new Vector3(90, 0, 0) - antenna.transform.localEulerAngles;
        targetTransform = targetAntenna.transform.GetChild(0);
        targetTransformAntenna = targetTransform.GetChild(0);

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = 0.1f * lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
        
    }

    void Update()
    {
        float distance = Vector3.Distance(antenna.position, targetTransformAntenna.position);
        sphere.transform.LookAt(targetTransform);
        Vector3 rotation = sphere.transform.eulerAngles;
        sphere.transform.rotation = Quaternion.Euler(rotation + offset);

        //Debug.DrawRay(antenna.transform.position, antenna.transform.up * distance, Color.green); //DEBUGGING
        
        lineRenderer.SetPosition(0, antenna.transform.position);
        lineRenderer.SetPosition(1, targetTransformAntenna.transform.position);

        if(this.isEnabled && lineRenderer.startColor != Color.green)
        {
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        } else if (!this.isEnabled && lineRenderer.startColor != Color.red)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
        }
    }

    private void HandleAntennaEnabled(int triggerId)
    {
        if(activatedByTriggerId.Contains(triggerId))
        {
            EventsManager.instance.OnAntennaEnable(this.triggerId1);
            EventsManager.instance.OnAntennaEnable(this.triggerId2);
            this.isEnabled = true;
        }
    }

    private void HandleAntennaDisabled(int triggerId)
    {
        if (activatedByTriggerId.Contains(triggerId))
        {
            EventsManager.instance.OnAntennaDisable(this.triggerId1);
            EventsManager.instance.OnAntennaDisable(this.triggerId2);
            this.isEnabled = false;
        }
    }
}
