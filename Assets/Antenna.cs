using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antenna : MonoBehaviour
{
    public GameObject targetAntenna;
    public float rotationSpeed = 2f;
    private Transform sphere;
    private Transform antenna;
    float strength = 0.5f;
    Vector3 offset = Vector3.zero;
    private Transform targetTransform;
    private Transform targetTransformAntenna;

    void Start()
    {
        //go = InputController.instance.hit.transform.gameObject;
        //transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y - 5, transform.parent.position.z);
        antenna = transform.GetChild(0).GetChild(0).transform;
        sphere = transform.GetChild(0).transform;
        Debug.Log(sphere);
        Debug.Log(antenna);

        
        offset = new Vector3(90, 0, 0) - antenna.transform.localEulerAngles;
        targetTransform = targetAntenna.transform.GetChild(0);
        targetTransformAntenna = targetTransform.GetChild(0);
    }

    void Update()
    {
        float distance = Vector3.Distance(antenna.position, targetTransformAntenna.position);
        sphere.transform.LookAt(targetTransform);
        Vector3 rotation = sphere.transform.eulerAngles;
        sphere.transform.rotation = Quaternion.Euler(rotation + offset);

        Debug.DrawRay(antenna.transform.position, antenna.transform.up * distance, Color.green); //DEBUGGING
        //Debug.DrawRay(sphere.transform.position, sphere.transform.forward * 100, Color.blue); //DEBUGGING
    }
}
