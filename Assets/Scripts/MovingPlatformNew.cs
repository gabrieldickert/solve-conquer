using UnityEngine;
using UnityEngine.AI;

public class MovingPlatformNew : MonoBehaviour
{

    public Vector3[] points;
    public Vector3[] rotations;
   
    private Vector3 current_target;
    private Quaternion current_rotation;

    public float speed = 1;
    public float rotation_speed = 1;
    public float delay_time = 0;
    public int point_number = 0;

    private float delay_start;
    public bool movesBidirectionally = false;
    public bool waitOnlyAtStartAndFinish = false;

    /*
    MovingPlatform modes
    0: no passengers required to start moving
    1: player required required to start moving, companion must not be passenger
    2: companion required to start moving, player must not be passenger
    3: player and companion required to start moving
    */
    public int mode = 0;
    public GameObject VisualTrigger1;
    public GameObject VisualTrigger2;
    private bool playerOnPlatform = false;
    private bool companionOnPlatform = false;
    private bool hasRequiredPassengers;
    private bool isReturning = false;

    private Transform companion = null;

    private Color companionTriggerColorInitial;
    private Color playerTriggerColorInitial;
    private float triggerActiveAlpha = 60f;

    // Start is called before the first frame update
    void Start()
    {
        if(points.Length != rotations.Length)
        {
            Debug.LogError("MovingPlatformNew: The arrays points and rotations have to have the same length.");
        }
        companionTriggerColorInitial = VisualTrigger2.GetComponent<MeshRenderer>().material.color;
        playerTriggerColorInitial = VisualTrigger1.GetComponent<MeshRenderer>().material.color;
        UpdateTarget();
        switch(mode)
        {
            case 0:
                this.hasRequiredPassengers = true;
                VisualTrigger1.SetActive(false);
                VisualTrigger2.SetActive(false);
                break;
            case 1:
                VisualTrigger2.SetActive(false);
                break;
            case 2:
                VisualTrigger1.SetActive(false);
                break;
            default:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        this.UpdateVisualTriggerTransparency();
        if (hasRequiredPassengers)
        {
            if (Time.time - delay_start > delay_time)
            {
                
                //Debug.Log("MovingPlatformNew: Moving towards next destination, " + (Time.time - delay_start));
                MovePlatform();
            }
        } else
        {
            this.delay_start = Time.time;
        }
        
    }

    void MovePlatform()
    {
        if(this.companion != null)
        {
            this.companion.GetComponent<NavMeshAgent>().enabled = false;
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, current_target, speed * Time.deltaTime);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, current_rotation, rotation_speed * Time.deltaTime);
        if (Vector3.Distance(transform.localPosition, current_target) == 0f && transform.localRotation == current_rotation)
        {
            UpdateTarget();
        }
        
    }

    

    public void UpdateTarget()
    {
        if(!waitOnlyAtStartAndFinish)
        {
            delay_start = Time.time;
            //Debug.Log("MovingPlatformNew: Delay from now");
        }
        if(!isReturning && point_number + 1 < points.Length)
        {
            point_number++;
            //Debug.Log("MovingPlatformNew: Going to next point");
        } else if(isReturning && point_number - 1 >= 0)
        {
            point_number--;
            //Debug.Log("MovingPlatformNew: Going to previous point");
        } else
        {
            //Debug.Log("MovingPlatformNew: Starting to backtrace");
            if (this.companion != null)
            {
                EventsManager.instance.OnCompanionFollow();
                this.companion.GetComponent<NavMeshAgent>().enabled = true;
            }
            if(movesBidirectionally)
            {
                point_number = isReturning ? point_number + 1 : point_number - 1;
                isReturning = !isReturning;
                if (waitOnlyAtStartAndFinish)
                {
                    delay_start = Time.time;
                    //Debug.Log("MovingPlatformNew: Delay from now, waitOnlyAtStartAndFinish is true");
                }
            }
        }
        current_target = points[point_number];
        current_rotation = Quaternion.Euler(rotations[point_number]);
        //Debug.Log("MovingPlatformNew: Current target is " + current_target + " [" + point_number + "/" + points.Length + "]");
    }

    private void UpdatePassengerStatus()
    {
        switch (mode)
        {
            case 1:
                this.hasRequiredPassengers = this.playerOnPlatform && !this.companionOnPlatform;
                break;
            case 2:
                this.hasRequiredPassengers = this.companionOnPlatform && !this.playerOnPlatform;
                break;
            case 3:
                this.hasRequiredPassengers = this.playerOnPlatform && this.companionOnPlatform;
                break;
        }
    }

    private void UpdateVisualTriggerTransparency()
    {
        Material playerTriggerMaterial = VisualTrigger1.GetComponent<MeshRenderer>().material;
        Material companionTriggerMaterial = VisualTrigger2.GetComponent<MeshRenderer>().material;
        
        if (this.playerOnPlatform && playerTriggerMaterial.color.a != this.triggerActiveAlpha)
        {
            playerTriggerMaterial.color = new Color(playerTriggerMaterial.color.r, playerTriggerMaterial.color.g, playerTriggerMaterial.color.b, this.triggerActiveAlpha);
        } else if(!this.playerOnPlatform && playerTriggerMaterial.color.a != playerTriggerColorInitial.a)
        {
            playerTriggerMaterial.color = playerTriggerColorInitial;
        }
        
        if(this.companionOnPlatform && companionTriggerMaterial.color.a != this.triggerActiveAlpha)
        {
            companionTriggerMaterial.color = new Color(companionTriggerMaterial.color.r, companionTriggerMaterial.color.g, companionTriggerMaterial.color.b, this.triggerActiveAlpha);
        } else if (!this.companionOnPlatform && companionTriggerMaterial.color.a != companionTriggerColorInitial.a)
        {
            companionTriggerMaterial.color = companionTriggerColorInitial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Companion")
        {
            companionOnPlatform = true;
            this.companion = other.transform;
            this.companion.SetParent(gameObject.transform);
            EventsManager.instance.OnCompanionWaitAt(VisualTrigger2.GetComponent<MeshRenderer>().bounds.center);
        }
        else if (other.gameObject.tag == "Player")
        {
            playerOnPlatform = true;
            other.transform.SetParent(gameObject.transform);
        }
        UpdatePassengerStatus();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Companion")
        {
            other.transform.parent.SetParent(null);
            this.companionOnPlatform = false;
        }
        else if (other.tag == "Player")
        {
            other.transform.SetParent(null);
            this.playerOnPlatform = false;
        }
        UpdatePassengerStatus();
    }
}
