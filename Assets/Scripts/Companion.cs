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

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        companionRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        //Debug.Log(transform.GetChild(0).GetComponent<MeshRenderer>());
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            this.isFollowing = !this.isFollowing;
            companionRenderer.material.color = isFollowing ? Color.green : Color.red;
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            companionRenderer.material.color = Color.yellow;
        }

        if (this.isFollowing)
        {
            //Follow the player
            agent.destination = transformToFollow.position;
        } else
        {
            agent.destination = agent.transform.position;
        }

        
    }
}