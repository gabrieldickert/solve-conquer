
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    //Transform that NPC has to follow
    public GameObject gameObjectToFollow;
    public float throwForce = 1f;

    private float maxDistanceFromDestination = 3f;
    private NavMeshAgent agent;
    private Renderer companionRenderer;
    private bool isFollowing = true;
    private float stoppingDistance = 0f;
    private Process process = new Process();
    private GameObject targetObject;
    private UnityEngine.Vector3 targetPosition;
    private GameObject carriedObject = null;
    private GameObject hackedObject = null;

    Animator animator;

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

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (agent.path.status == NavMeshPathStatus.PathInvalid)
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            return;
        }

        agent.SetDestination(targetPosition);
        
        switch (process.CurrentState)
        {
            case ProcessState.Following:
                agent.stoppingDistance = this.stoppingDistance;
                agent.isStopped = false;
                companionRenderer.material.color = Color.green;
                this.targetObject = this.gameObjectToFollow;
                this.targetPosition = this.gameObjectToFollow.transform.position;
                
                animator.SetBool("isMoving", true);

                if(this.carriedObject != null && this.carriedObject.transform.position != gameObject.transform.position + new Vector3(0f, 2f, 0f))
                {
                    this.Drop(this.carriedObject);
                }
                break;
            case ProcessState.WaitingAt:
                agent.isStopped = false;
                animator.SetBool("isMoving", false);
                break;
            case ProcessState.Fetching:
                agent.isStopped = false;
                if (Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination)
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
                if (Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination)
                {
                    agent.isStopped = true;
                    process.MoveNext(Command.CompleteHack);
                }
                break;
            case ProcessState.HackCompleted:
                this.Hack(targetObject);
                break;
            case ProcessState.AbortingHack:
                agent.isStopped = false;
                this.StopHack(this.hackedObject);
                process.MoveNext(process.LastCommand);
                break;
        }
    }
    void HandleCompanionWaitAt(UnityEngine.Vector3 waitingPosition)
    {
        process.MoveNext(Command.WaitAt);
        if(process.CurrentState == ProcessState.WaitingAt || process.CurrentState == ProcessState.AbortingHack)
        {
            companionRenderer.material.color = Color.red;
            this.targetObject = null;
            this.targetPosition = waitingPosition;
            agent.stoppingDistance = 0f;
        }
    }

    void HandleCompanionPickUpObject(GameObject targetObject)
    {
        process.MoveNext(Command.Fetch);
        if (process.CurrentState == ProcessState.Fetching || process.CurrentState == ProcessState.AbortingHack)
        {
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
        if (process.CurrentState == ProcessState.Hacking || process.CurrentState == ProcessState.AbortingHack)
        {
            companionRenderer.material.color = Color.yellow;
            this.targetObject = targetObject;
            this.targetPosition = targetObject.transform.position;
            agent.stoppingDistance = 0f;
        }
    }

    void HandleCompanionFollow()
    {
        process.MoveNext(Command.Follow);
        animator.SetBool("isMoving", true);
    }

   

    void PickUp(GameObject targetObject)
    {
        if(this.carriedObject == null)
        {
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
            this.carriedObject = null;
            EventsManager.instance.OnForceObjectBarrierDisableObstacle();
            targetObject.GetComponent<Rigidbody>().isKinematic = false;
            targetObject.transform.parent = null;
            targetObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * this.throwForce);
        }
    }

    void Hack(GameObject targetGameObject)
    {
        companionRenderer.material.color = Color.black;
        this.hackedObject = targetGameObject;
        EventsManager.instance.OnCompanionHackEnable(this.hackedObject.GetInstanceID());
    }

    void StopHack(GameObject targetGameObject)
    {
        EventsManager.instance.OnCompanionHackDisable(targetGameObject.GetInstanceID());
        this.hackedObject = null;
    }
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
    public Command LastCommand { get; set; }

    public Process()
    {
        CurrentState = ProcessState.Following;
        LastCommand = Command.Follow;
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
                { new StateTransition(ProcessState.AbortingHack, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.AbortingHack, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.AbortingHack, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.Fetching, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.Hacking, Command.WaitAt), ProcessState.WaitingAt },
                { new StateTransition(ProcessState.HackCompleted, Command.WaitAt), ProcessState.AbortingHack },
                { new StateTransition(ProcessState.WaitingAt, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.Hacking, Command.Fetch), ProcessState.Fetching },
                { new StateTransition(ProcessState.HackCompleted, Command.Fetch), ProcessState.AbortingHack },
                { new StateTransition(ProcessState.WaitingAt, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.Fetching, Command.Hack), ProcessState.Hacking },
                { new StateTransition(ProcessState.HackCompleted, Command.Hack), ProcessState.AbortingHack },
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
        {
            throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
        } else
        {
            LastCommand = command;
            return nextState;
        }
    }

    public ProcessState MoveNext(Command command)
    {
        CurrentState = GetNext(command);
        return CurrentState;
    }
}
