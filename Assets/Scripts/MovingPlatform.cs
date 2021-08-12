using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingPlatform : MonoBehaviour
{

    public Vector3[] points;
    public int point_number = 0;
    private Vector3 current_target;

    public float tolerance;
    public float speed;
    public float delay_time;

    private float delay_start;
    public bool automatic;

    public Vector3[] eulers;
    public int euler_number = 0;
    private Vector3 current_euler;
    public float rotation_speed;

    public int mode = 0;
    public GameObject VisualTrigger1;
    public GameObject VisualTrigger2;
    private bool playerOnPlatform = false;
    private bool companionOnPlatform = false;

    private Transform companion = null;

    // Start is called before the first frame update
    void Start()
    {
        if(points.Length > 0){
            current_target = points[0];
        }
        tolerance = speed * Time.deltaTime;
        
        if(eulers.Length > 0){
            current_euler = eulers[0];
        }

        switch (mode)
        {
            case 0:
                VisualTrigger1.GetComponent<MeshRenderer>().enabled = false;
                VisualTrigger2.GetComponent<MeshRenderer>().enabled = false;
                break;
            case 1:
                VisualTrigger2.GetComponent<MeshRenderer>().enabled = false;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != current_target){
            if(Time.time - delay_start > delay_time){
                MovePlatform();
            } else
            {
                if (this.companion != null)
                {
                    this.companion.GetComponent<NavMeshAgent>().enabled = false;
                    this.companion.GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
        else{
            UpdateTarget();
        }
    }

    void MovePlatform(){
        Vector3 heading = current_target - transform.localPosition;
        transform.localPosition += (heading/ heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance){
            transform.localPosition = current_target;
            delay_start = Time.time;
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(current_euler), Time.deltaTime * rotation_speed);
    }

    void UpdateTarget(){
        if (this.companion != null)
        {
            this.companion.GetComponent<NavMeshAgent>().enabled = true;
            this.companion.GetComponent<Rigidbody>().isKinematic = false;
        }
        if (automatic){
            if(Time.time - delay_start > delay_time){
                NextPlatform();
            }
        }
    }

    public void NextPlatform(){
        
        point_number++;
        if(point_number >= points.Length){
            point_number = 0;
        }
        current_target = points[point_number];

        euler_number++;
        if(euler_number >= eulers.Length){
            euler_number = 0;
        }
        
        current_euler = eulers[euler_number];
    }

    private void OnTriggerEnter(Collider other){
        

        if (other.gameObject.tag == "Companion")
        {
            companionOnPlatform = true;
            this.companion = other.transform.parent;
            this.companion.SetParent(gameObject.transform);
        }
        else if (other.gameObject.tag == "Player")
        {
            playerOnPlatform = true;
            other.transform.SetParent(gameObject.transform);
        }
        switch (mode)
        {
            case 0:
                NextPlatform();
                break;
            case 1:
                if (playerOnPlatform)
                {
                    NextPlatform();
                }
                break;
            case 2:
                if (playerOnPlatform && companionOnPlatform)
                {
                    NextPlatform();
                }
                break;
        }
    }

    private void OnTriggerExit(Collider other){
        if (other.tag == "Companion")
        {
            other.transform.parent.SetParent(null);
        }
        else if (other.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
