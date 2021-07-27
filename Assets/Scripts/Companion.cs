
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    //Transform that NPC has to follow
    public GameObject gameObjectToFollow;
    public float throwForce = 1f;
    private float maxDistanceFromDestination = 2f;

    //NavMesh Agent variable
    NavMeshAgent agent;

    private Renderer companionRenderer;
    private bool isFollowing = true;
    private float stoppingDistance = 0f;
    private Process process = new Process();

    private GameObject targetObject;
    private UnityEngine.Vector3 targetPosition;
    private GameObject carriedObject = null;

    //bool to check if state changed last update
    //used to avoid continuous agent parameter updating while state remains the same
    //(only useful in states that can linger)
    //private bool didChangeStateLastUpdate = true;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.CompanionWaitAt += HandleCompanionWaitAt;
        EventsManager.instance.CompanionPickUpObject += HandleCompanionPickUpObject;
        EventsManager.instance.CompanionHackObject += HandleCompanionHackObject;
        EventsManager.instance.CompanionFollow += HandleCompanionFollow;
        EventsManager.instance.CompanionDropObject += HandleCompanionDropObject;
        agent = GetComponent<NavMeshAgent>();
        stoppingDistance = agent.stoppingDistance;
        companionRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
        companionRenderer.material.color = isFollowing ? Color.green : Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CompanionState: " + process.CurrentState);

        if (agent.path.status == NavMeshPathStatus.PathInvalid)
        {
            Debug.Log("Companion: Path invalid. Respawning.");
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            return;
        }

        agent.SetDestination(targetPosition);
        
        switch (process.CurrentState)
        {
            case ProcessState.Following:
                //if(this.didChangeStateLastUpdate)
                //{
                    Debug.Log("CompanionState changed to Following");
                    agent.stoppingDistance = this.stoppingDistance;
                    agent.isStopped = false;
                    companionRenderer.material.color = Color.green;
                    this.targetObject = this.gameObjectToFollow;
                    //this.didChangeStateLastUpdate = false;
                //}
                this.targetPosition = this.gameObjectToFollow.transform.position;
                break;
            case ProcessState.WaitingAt:
                agent.isStopped = false;
                break;
            case ProcessState.Fetching:
                agent.isStopped = false;
                if (UnityEngine.Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination)
                {
                    agent.isStopped = true;
                    PickUp(targetObject);
                    process.MoveNext(Command.PickUp);
                }
                break;
            case ProcessState.PickedUp:
                agent.isStopped = false;
                Drop(this.carriedObject);
                PickUp(this.targetObject);
                process.MoveNext(Command.Follow);
                break;
            case ProcessState.Hacking:
                Drop(this.carriedObject);
                agent.isStopped = false;
                if (UnityEngine.Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination)
                {
                    //agent.isStopped = true;
                    process.MoveNext(Command.CompleteHack);
                }
                break;
            case ProcessState.HackCompleted:
                //if(this.didChangeStateLastUpdate)
                //{
                    //this.didChangeStateLastUpdate = false;
                    //hack
                    Hack(targetObject);
                //}
                break;
            case ProcessState.AbortingHack:
                agent.isStopped = false;
                process.MoveNext(Command.Follow);
                break;
        }
    }



    /*void HandleCompanionWaitAt(Vector3 waitingPosition)
    {
        this.isFollowing = false;
        companionRenderer.material.color = Color.red;
        agent.destination = waitingPosition;
        agent.stoppingDistance = 0f;
    }*/

    void HandleCompanionWaitAt(UnityEngine.Vector3 waitingPosition)
    {
        process.MoveNext(Command.WaitAt);
        if(process.CurrentState == ProcessState.WaitingAt)
        {
            //this.didChangeStateLastUpdate = true;
            companionRenderer.material.color = Color.red;
            this.targetObject = null;
            this.targetPosition = waitingPosition;
            agent.stoppingDistance = 0f;
        }
    }

    void HandleCompanionPickUpObject(GameObject targetObject)
    {
        process.MoveNext(Command.Fetch);
        if (process.CurrentState == ProcessState.Fetching)
        {
            //this.didChangeStateLastUpdate = true;
            companionRenderer.material.color = Color.blue;
            this.targetObject = targetObject;
            this.targetPosition = targetObject.transform.position;
            agent.stoppingDistance = 0f;
        }
    }

    void HandleCompanionDropObject()
    {
        Drop(this.carriedObject);
    }

    void HandleCompanionHackObject(GameObject targetObject)
    {
        process.MoveNext(Command.Hack);
        if (process.CurrentState == ProcessState.Hacking)
        {
            //this.didChangeStateLastUpdate = true;
            companionRenderer.material.color = Color.yellow;
            this.targetObject = targetObject;
            this.targetPosition = targetObject.transform.position;
            agent.stoppingDistance = 0f;
        }
    }

    void HandleCompanionFollow()
    {

        process.MoveNext(Command.Follow);
        if(process.CurrentState == ProcessState.Following)
        {
            //this.didChangeStateLastUpdate = true;
        }
        /*if (process.CurrentState == ProcessState.Fetching)
        {
            companionRenderer.material.color = Color.blue;
            agent.stoppingDistance = this.stoppingDistance;
            this.targetObject = this.gameObjectToFollow;
            this.targetPosition = this.gameObjectToFollow.transform.position;
        }*/
    }

    /*void HandleCompanionPickUpObject(GameObject targetObject)
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
    }*/

    void PickUp(GameObject targetObject)
    {
        if(this.carriedObject == null)
        {
            //aufheben
            companionRenderer.material.color = Color.white;
            this.carriedObject = targetObject;
            EventsManager.instance.OnForceObjectBarrierEnableObstacle();
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
            targetObject.transform.parent = gameObject.transform;
            targetObject.transform.position = gameObject.transform.position + new UnityEngine.Vector3(0f, 2f, 0f);
        }
    }

    void Drop(GameObject targetObject)
    {
        if(this.carriedObject != null)
        {
            //fallen lassen
            this.carriedObject = null;
            EventsManager.instance.OnForceObjectBarrierDisableObstacle();
            targetObject.GetComponent<Rigidbody>().isKinematic = false;
            targetObject.transform.parent = null;
            targetObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * this.throwForce);
        }
    }

    void Hack(GameObject targetObject)
    {
        //hacken
        companionRenderer.material.color = Color.black;
    }
    /*
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
    }*/

    
}

public enum ProcessState
{
    Following,
    Fetching,
    Hacking,
    WaitingAt,
    PickedUp,
    HackCompleted,
    AbortingHack
}

public enum Command
{
    Follow,
    Fetch,
    Hack,
    WaitAt,
    PickUp,
    CompleteHack
}

public class Process
{
    class StateTransition
    {
        readonly ProcessState CurrentState;
        readonly Command Command;

        public StateTransition(ProcessState currentState, Command command)
        {
            CurrentState = currentState;
            Command = command;
        }

        public override int GetHashCode()
        {
            return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            StateTransition other = obj as StateTransition;
            return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
        }
    }

    Dictionary<StateTransition, ProcessState> transitions;
    public ProcessState CurrentState { get; private set; }

    public Process()
    {
        CurrentState = ProcessState.Following;
        transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.Following, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.WaitingAt, Command.Follow), ProcessState.Following },
                { new StateTransition(ProcessState.Following, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.Fetching, Command.PickUp), ProcessState.PickedUp },
                { new StateTransition(ProcessState.Fetching, Command.Follow), ProcessState.Following },
                { new StateTransition(ProcessState.PickedUp, Command.Follow), ProcessState.Following },
                { new StateTransition(ProcessState.Following, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.Hacking, Command.CompleteHack), ProcessState.HackCompleted },
                { new StateTransition(ProcessState.Hacking, Command.Follow), ProcessState.Following },
                { new StateTransition(ProcessState.HackCompleted, Command.Follow), ProcessState.AbortingHack },
                { new StateTransition(ProcessState.AbortingHack, Command.Follow), ProcessState.Following },
                { new StateTransition(ProcessState.Fetching, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.Hacking, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.HackCompleted, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.WaitingAt, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.Hacking, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.HackCompleted, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.WaitingAt, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.Fetching, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.HackCompleted, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.WaitingAt, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.Fetching, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.Hacking, Command.Hack), ProcessState.Hacking }
            };
    }

    public ProcessState GetNext(Command command)
    {
        StateTransition transition = new StateTransition(CurrentState, command);
        ProcessState nextState;
        if (!transitions.TryGetValue(transition, out nextState))
            throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
        return nextState;
    }

    public ProcessState MoveNext(Command command)
    {
        CurrentState = GetNext(command);
        return CurrentState;
    }
}
