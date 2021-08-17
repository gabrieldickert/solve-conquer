
using OculusSampleFramework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    //Transform that NPC has to follow
    public GameObject gameObjectToFollow;
    public float throwForce = 10f;

    private float maxDistanceFromDestination = 2f;
    private NavMeshAgent agent;
    private Renderer companionRenderer;
    private bool isFollowing = true;
    private float stoppingDistance = 0f;
    private Process process = new Process();
    private GameObject targetObject;
    private UnityEngine.Vector3 targetPosition;
    private GameObject carriedObject = null;
    private GameObject hackedObject = null;
    private bool hasReachedTargetObject = false;

    Animator animator;
    private bool hasCurrentAnimationFinished = false;
    private string currentAnimation = "Idle";

    private Vector3 localCarryPosition = new Vector3(0f, 0.57f, 0.55f);
    private Quaternion localCarryRotation = Quaternion.identity;

    void Start()
    {
        EventsManager.instance.CompanionWaitAt += HandleCompanionWaitAt;
        EventsManager.instance.CompanionPickUpObject += HandleCompanionPickUpObject;
        EventsManager.instance.CompanionHackObject += HandleCompanionHackObject;
        EventsManager.instance.CompanionFollow += HandleCompanionFollow;
        EventsManager.instance.CompanionDropObject += HandleCompanionDropObject;
        
        agent = GetComponent<NavMeshAgent>();
        stoppingDistance = agent.stoppingDistance;
        companionRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        companionRenderer.material.color = isFollowing ? Color.green : Color.red;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        this.hasReachedTargetObject = Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination;
        Debug.Log("Companion: Distance to target = " + Vector3.Distance(agent.transform.position, targetPosition));
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !this.hasCurrentAnimationFinished)
        {
            this.hasCurrentAnimationFinished = true;
            Debug.Log("Companion: Current Animation has finished");
        }
        
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
                if(agent.velocity == Vector3.zero){
                    animator.Play(this.carriedObject == null ? "Idle" : "Pickup Idle");
                } else {
                    animator.Play(this.carriedObject == null ? "Walk" : "Pickup Walk");
                }

                /*if(this.carriedObject != null && this.carriedObject.transform.position != gameObject.transform.position + new Vector3(0f, 2f, 0f))
                {
                    this.Drop(this.carriedObject);
                }*/

                break;
            case ProcessState.WaitingAt:
                agent.isStopped = false;
                if (agent.velocity == Vector3.zero)
                {
                    animator.Play(this.carriedObject == null ? "Idle" : "Pickup Idle");
                }
                else
                {
                    animator.Play(this.carriedObject == null ? "Walk" : "Pickup Walk");
                }
                break;
            case ProcessState.Fetching:
                agent.isStopped = false;
                SetAnimation(this.carriedObject == null ? "Walk" : "Pickup Walk");
                //if (Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination)
                if (this.hasReachedTargetObject)
                {
                    agent.isStopped = true;
                    //PickUp(targetObject);
                    SetAnimation("Drop");
                    //if (this.hasCurrentAnimationFinished)
                    //{
                        Drop(this.carriedObject);
                        process.MoveNext(Command.PickUp);
                    //}
                }
                break;
            case ProcessState.PickedUp:
                agent.isStopped = false;
                this.hasReachedTargetObject = false;
                //Drop(this.carriedObject);
                SetAnimation("Pickup");
                if (this.hasCurrentAnimationFinished){
                    PickUp(this.targetObject);
                    process.MoveNext(Command.Follow);
                }
                break;
            case ProcessState.Hacking:
                //Drop(this.carriedObject);
                SetAnimation(this.carriedObject == null ? "Walk" : "Pickup Walk");
                agent.isStopped = false;
                //if (Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination)
                if(this.hasReachedTargetObject)
                {
                    agent.isStopped = true;
                    SetAnimation("Drop");
                    //if (this.hasCurrentAnimationFinished)
                    //{
                        Drop(this.carriedObject);
                        process.MoveNext(Command.CompleteHack);
                    //}
                }
                break;
            case ProcessState.HackCompleted:
                this.hasReachedTargetObject = false;
                SetAnimation("Pickup");
                //if (this.hasCurrentAnimationFinished)
                //{
                    this.Hack(targetObject);
                //}
                break;
            case ProcessState.AbortingHack:
                agent.isStopped = false;
                SetAnimation("Drop");
                //if (this.hasCurrentAnimationFinished)
                //{
                    this.StopHack(this.hackedObject);
                    process.MoveNext(process.LastCommand);
                //}
                break;
        }
    }

    private void SetAnimation(string animation)
    {
        if(this.currentAnimation != animation && this.hasCurrentAnimationFinished)
        {
            animator.Play(animation);
            this.currentAnimation = animation;
            this.hasCurrentAnimationFinished = false;
        }
        
    }
    
    /*private void OnTriggerStay(Collider other)
    {
        Debug.Log("Companion: collision with " + other.gameObject);
        if(process.CurrentState == ProcessState.Fetching || process.CurrentState == ProcessState.Hacking)
        {
            if(other.gameObject == this.targetObject)
            {
                this.hasReachedTargetObject = true;
            }
        }
    }*/
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
        //animator.SetBool("isMoving", true);
    }

   

    void PickUp(GameObject targetObject)
    {
        if(this.carriedObject == null)
        {
            //StartCoroutine(PickingUp());
            companionRenderer.material.color = Color.white;
            this.carriedObject = targetObject;
            EventsManager.instance.OnForceObjectBarrierEnableObstacle();
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
            targetObject.GetComponent<NavMeshObstacle>().enabled = false;
            targetObject.GetComponent<DistanceGrabbable>().enabled = false;
            targetObject.transform.parent = gameObject.transform;
            //targetObject.transform.localPosition = Vector3.MoveTowards(targetObject.transform.position, new Vector3(0f, 0.57f, 0.55f), Time.deltaTime);
            targetObject.transform.localPosition = this.localCarryPosition;
            targetObject.transform.localRotation = this.localCarryRotation;
        }
    }

    void Drop(GameObject targetObject)
    {
        if(this.carriedObject != null)
        {
            //animator.Play("Drop");
            this.carriedObject = null;
            EventsManager.instance.OnForceObjectBarrierDisableObstacle();
            
            targetObject.GetComponent<Rigidbody>().isKinematic = false;
            targetObject.transform.parent = null;
            targetObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * this.throwForce);
            targetObject.GetComponent<NavMeshObstacle>().enabled = true;
            targetObject.GetComponent<DistanceGrabbable>().enabled = true;
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

    /*private IEnumerator<void> PickingUp()
    {
        animator.SetTrigger("triggerPickup");

        while (!animator.IsInTransition(0))
        {
            yield return null;
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
