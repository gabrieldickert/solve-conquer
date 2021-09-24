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


    public int level;
    public int stage;
    public bool isActive;


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
    private bool playerOnPlatformTrigger = false;
    private bool companionOnPlatformTrigger = false;
    [HideInInspector] public bool hasRequiredPassengers;
    private bool hasUnwantedPassengers;
    private bool isReturning = false;

    private Transform companion = null;

    private Color companionTriggerColorInitial;
    private Color playerTriggerColorInitial;
    private float triggerActiveAlpha = 60f;

    private bool hasStopped = false;

    public Sound sound;

    private AudioSource AudioSrc;

    // Start is called before the first frame update
    void Start()
    {
        //Set up Audio Source
        this.AudioSrc = this.GetComponent<AudioSource>();
        this.AudioSrc.spatialBlend = 1.0f;

        if(points.Length != rotations.Length)
        {
            //Debug.LogError("MovingPlatformNew: The arrays points and rotations have to have the same length.");
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
        if (hasRequiredPassengers && !hasUnwantedPassengers)
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
        
        if (Vector3.Distance(transform.localPosition, current_target) == 0f && Vector3.Distance(transform.localRotation.eulerAngles, current_rotation.eulerAngles) < 0.1)
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
            if (!this.AudioSrc.isPlaying)
            {
                this.AudioSrc.loop = true;
                this.AudioSrc.clip = this.sound.clipList[0];
                this.AudioSrc.Play();

            }
            point_number++;
            //Debug.Log("MovingPlatformNew: Going to next point");
        } else if(isReturning && point_number - 1 >= 0)
        {
            if (!this.AudioSrc.isPlaying)
            {
                this.AudioSrc.loop = true;
                this.AudioSrc.clip = this.sound.clipList[0];
                this.AudioSrc.Play();

            }
            point_number--;
            //Debug.Log("MovingPlatformNew: Going to previous point");
        } else
        {
            //Debug.Log("MovingPlatformNew: Starting to backtrace");
            if (this.companion != null)
            {
                if(mode == 2 || mode == 3)
                {
                    EventsManager.instance.OnCompanionFollow();
                }
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
            } else
            {
                if(!this.hasStopped)
                {
                    this.AudioSrc.Stop();
                    this.AudioSrc.loop = false;
                    this.AudioSrc.clip = this.sound.clipList[1];
                    this.AudioSrc.Play();
                }
 
                this.hasStopped = true;
            }
        }
        current_target = points[point_number];
        current_rotation = Quaternion.Euler(rotations[point_number]);
        //Debug.Log("MovingPlatformNew: Current target is " + current_target + " [" + point_number + "/" + points.Length + "]");
    }

    public void UpdatePassengerStatus()
    {
        switch (mode)
        {
            case 1:
                this.hasRequiredPassengers = this.playerOnPlatformTrigger && !this.companionOnPlatformTrigger;
                break;
            case 2:
                this.hasRequiredPassengers = this.companionOnPlatformTrigger && !this.playerOnPlatformTrigger;
                break;
            case 3:
                this.hasRequiredPassengers = this.playerOnPlatformTrigger && this.companionOnPlatformTrigger;
                break;
        }
    }

    private void UpdateVisualTriggerTransparency()
    {
        Material playerTriggerMaterial = VisualTrigger1.GetComponent<MeshRenderer>().material;
        Material companionTriggerMaterial = VisualTrigger2.GetComponent<MeshRenderer>().material;
        
        if (this.playerOnPlatformTrigger && playerTriggerMaterial.color.a != this.triggerActiveAlpha)
        {
            playerTriggerMaterial.color = new Color(playerTriggerMaterial.color.r, playerTriggerMaterial.color.g, playerTriggerMaterial.color.b, this.triggerActiveAlpha);
        } else if(!this.playerOnPlatformTrigger && playerTriggerMaterial.color.a != playerTriggerColorInitial.a)
        {
            playerTriggerMaterial.color = playerTriggerColorInitial;
        }
        
        if(this.companionOnPlatformTrigger && companionTriggerMaterial.color.a != this.triggerActiveAlpha)
        {
            companionTriggerMaterial.color = new Color(companionTriggerMaterial.color.r, companionTriggerMaterial.color.g, companionTriggerMaterial.color.b, this.triggerActiveAlpha);
        } else if (!this.companionOnPlatformTrigger && companionTriggerMaterial.color.a != companionTriggerColorInitial.a)
        {
            companionTriggerMaterial.color = companionTriggerColorInitial;
        }
    }

    public void OnPlayerEnteredTrigger(Collider other)
    {
        playerOnPlatformTrigger = true;
        //other.transform.SetParent(gameObject.transform);
        UpdatePassengerStatus();
    }

    public void OnCompanionEnteredTrigger(Collider other)
    {
        companionOnPlatformTrigger = true;
        //this.companion = other.transform;
        //this.companion.SetParent(gameObject.transform);
        UpdatePassengerStatus();
    }

    public void OnPlayerLeftTrigger(Collider other)
    {
        //other.transform.SetParent(null);
        this.playerOnPlatformTrigger = false;
        UpdatePassengerStatus();
    }

    public void OnCompanionLeftTrigger(Collider other)
    {
        //other.transform.parent.SetParent(null);
        this.companionOnPlatformTrigger = false;
        UpdatePassengerStatus();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Companion")
        {
            if ((mode == 2 || mode == 3) && !this.hasStopped)
            {
                EventsManager.instance.OnCompanionWaitAt(VisualTrigger2.GetComponent<MeshRenderer>().bounds.center);
            } else if(mode == 1)
            {
                this.hasUnwantedPassengers = true;
            }
            this.companion = other.transform;
            other.transform.SetParent(gameObject.transform);
        }
        if(other.gameObject.tag == "Player")
        {
            if(mode == 2)
            {
                this.hasUnwantedPassengers = true;
            }
            other.transform.SetParent(gameObject.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Companion")
        {
           if (mode == 1)
           {
                this.hasUnwantedPassengers = false;
           }
            this.companion = null;
            other.transform.SetParent(null);
        }
        if (other.gameObject.tag == "Player")
        {
            if (mode == 2)
            {
                this.hasUnwantedPassengers = false;
            }
            other.transform.SetParent(null);
        }
    }

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Companion")
        {

        }
    }*/

    /*private void OnTriggerEnter(Collider other)
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
    }*/
}
