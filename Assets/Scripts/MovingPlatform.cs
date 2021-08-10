using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition != current_target){
            MovePlatform();
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
        if(automatic){
            if(Time.time - delay_start > delay_time){
                NextPlatform();
            }
        }
    }

    void NextPlatform(){
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
        other.transform.parent = transform;
        /*if(other.gameObject.tag == "Player"){
            other.transform.parent = transform;
        }*/
    }

    private void OnTriggerExit(Collider other){
        other.transform.parent = null;
        /*if(other.gameObject.tag == "Player"){
            other.transform.parent = null;
        }*/
    }
}
