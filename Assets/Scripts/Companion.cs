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
        agent = GetComponent<NavMeshAgent>();
        stoppingDistance = agent.stoppingDistance;
        companionRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        companionRenderer.material.color = isFollowing ? Color.green : Color.red;
        //Debug.Log(transform.GetChild(0).GetComponent<MeshRenderer>());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //this.isFollowing = !this.isFollowing;
            //companionRenderer.material.color = isFollowing ? Color.green : Color.red;
            this.isFollowing = true;
            agent.stoppingDistance = stoppingDistance;
        }
        /*
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            companionRenderer.material.color = Color.yellow;
            
        }
        */

        if (this.isFollowing)
        {
            //Follow the player
            agent.destination = transformToFollow.position;
            if(companionRenderer.material.color != Color.green)
            {
                companionRenderer.material.color = Color.green;
            }
        } 
    }

    void HandleCompanionWaitAt(Vector3 waitingPosition)
    {
        this.isFollowing = false;
        companionRenderer.material.color = Color.red;
        agent.destination = waitingPosition;
        agent.stoppingDistance = 0f;
    }
}