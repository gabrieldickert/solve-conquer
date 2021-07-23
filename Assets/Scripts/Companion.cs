
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    //Transform that NPC has to follow
    public Transform transformToFollow;
    //NavMesh Agent variable
    NavMeshAgent agent;

    private Renderer companionRenderer;
    private bool isFollowing = true;
    private float stoppingDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.CompanionWaitAt += HandleCompanionWaitAt;
        EventsManager.instance.CompanionPickUpObject += HandleCompanionPickUpObject;
        EventsManager.instance.CompanionHackObject += HandleCompanionHackObject;
        EventsManager.instance.CompanionFollow += HandleCompanionFollow;
        agent = GetComponent<NavMeshAgent>();
        stoppingDistance = agent.stoppingDistance;
        companionRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        companionRenderer.material.color = isFollowing ? Color.green : Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<NavMeshAgent>().enabled)
        {
            Debug.Log("pathstatus = " + agent.pathStatus);
            if (agent.pathStatus.Equals(NavMeshPathStatus.PathInvalid))
            {
                Debug.Log("Companion: Agent seems to be floating in the air. Disabling agent.");
                gameObject.GetComponent<NavMeshAgent>().enabled = false;
            }
            if (this.isFollowing && gameObject.GetComponent<NavMeshAgent>().enabled)
            {
                //Follow the player
                agent.destination = transformToFollow.position;
            }
        } else
        {
            Debug.Log("Companion: agent disabled");
        }
        

       
    }

    void HandleCompanionWaitAt(Vector3 waitingPosition)
    {
        this.isFollowing = false;
        companionRenderer.material.color = Color.red;
        agent.destination = waitingPosition;
        agent.stoppingDistance = 0f;
    }

    void HandleCompanionPickUpObject(GameObject targetObject)
    {
        this.isFollowing = false;
        companionRenderer.material.color = Color.blue;
        agent.destination = targetObject.transform.position;
        agent.stoppingDistance = 0f;
    }

    void HandleCompanionHackObject(GameObject targetObject)
    {
        this.isFollowing = false;
        companionRenderer.material.color = Color.yellow;
        agent.destination = targetObject.transform.position;
        agent.stoppingDistance = 0f;
    }

    void HandleCompanionFollow()
    {
        this.isFollowing = true;
        agent.stoppingDistance = stoppingDistance;
        companionRenderer.material.color = Color.green;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        
        //this.hasCollision = true;
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "GrabbableObject" || collision.gameObject.tag == "HackableObject")
        {
            Debug.Log("Companion collided with an object with tag " + collision.gameObject.tag);
            this.isFollowing = false;
            agent.destination = agent.transform.position;
            companionRenderer.material.color = Color.red;
        } 
    }

    
}