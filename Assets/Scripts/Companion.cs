
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
    
    /*required Order in audioClips array
     * 0: idle
     * 1: drive
     * 2: follow
     * 3: waitAt
     * 4: fetch
     * 5: hack
     * 6: hacking
     * 7: drop
     */
    public AudioClip[] audioClips;

    private AudioSource audioSrc;
    private bool isPlayingIdleLoop = false;

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
    public float pickUpSpeed = 5f;
    public float rotationSpeed = 1f;
    private bool isCarriedObjectFullyPickedUp = false;
    private bool isCarriedObjectFullyRotated = false;

    void Start()
    {
        EventsManager.instance.CompanionWaitAt += HandleCompanionWaitAt;
        EventsManager.instance.CompanionPickUpObject += HandleCompanionPickUpObject;
        EventsManager.instance.CompanionHackObject += HandleCompanionHackObject;
        EventsManager.instance.CompanionFollow += HandleCompanionFollow;
        EventsManager.instance.CompanionDropObject += HandleCompanionDropObject;

        audioSrc = this.GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        stoppingDistance = agent.stoppingDistance;
        companionRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
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
        this.hasReachedTargetObject = Vector3.Distance(agent.transform.position, targetPosition) < this.maxDistanceFromDestination;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !this.hasCurrentAnimationFinished)
        {
            this.hasCurrentAnimationFinished = true;
        }

        if (this.carriedObject != null && (!this.isCarriedObjectFullyPickedUp || !this.isCarriedObjectFullyRotated))
        {
            if(this.carriedObject.transform.localPosition != this.localCarryPosition)
            {
                carriedObject.transform.localPosition = Vector3.MoveTowards(carriedObject.transform.localPosition, localCarryPosition, this.pickUpSpeed * Time.deltaTime);
            } else
            {
                this.isCarriedObjectFullyPickedUp = true;
            }

            if (this.carriedObject.transform.localRotation != this.localCarryRotation)
            {
                this.carriedObject.transform.localRotation = Quaternion.Slerp(this.carriedObject.transform.localRotation, localCarryRotation, this.rotationSpeed * Time.deltaTime);
            } else
            {
                this.isCarriedObjectFullyRotated = true;
            }
        } else if (this.carriedObject != null && this.carriedObject.transform.localPosition != this.localCarryPosition) {
            Drop(this.carriedObject);
        }

        if (agent.velocity == Vector3.zero)
        {
            if (!isPlayingIdleLoop)
            {
                PlaySoundFX(audioClips[0], true);
                isPlayingIdleLoop = true;
            }
        }
        else
        {
            if (isPlayingIdleLoop)
            {
                PlaySoundFX(audioClips[1], true);
                isPlayingIdleLoop = false;
            }
        }

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
                if (this.hasReachedTargetObject)
                {
                    agent.isStopped = true;
                    SetAnimation("Drop");
                    Drop(this.carriedObject);
                    process.MoveNext(Command.PickUp);
                }
                break;
            case ProcessState.PickedUp:
                agent.isStopped = false;
                this.hasReachedTargetObject = false;
                SetAnimation("Pickup");
                if (this.hasCurrentAnimationFinished){
                    PickUp(this.targetObject);
                    process.MoveNext(Command.Follow);
                }
                break;
            case ProcessState.Hacking:
                SetAnimation(this.carriedObject == null ? "Walk" : "Pickup Walk");
                agent.isStopped = false;
                if(this.hasReachedTargetObject)
                {
                    agent.isStopped = true;
                    SetAnimation("Drop"); 
                    Drop(this.carriedObject);
                    process.MoveNext(Command.CompleteHack);
                }
                break;
            case ProcessState.HackCompleted:
                this.hasReachedTargetObject = false;
                SetAnimation("Pickup");
                this.Hack(targetObject);
                break;
            case ProcessState.AbortingHack:
                agent.isStopped = false;
                SetAnimation("Drop");
                this.StopHack(this.hackedObject);
                process.MoveNext(process.LastCommand);
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
    
    void HandleCompanionWaitAt(UnityEngine.Vector3 waitingPosition)
    {
        process.MoveNext(Command.WaitAt);
        PlaySoundFX(audioClips[3], false);
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
        PlaySoundFX(audioClips[4], false);
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
        PlaySoundFX(audioClips[7], false);
        Drop(this.carriedObject);
    }

    void HandleCompanionHackObject(GameObject targetObject)
    {
        PlaySoundFX(audioClips[5], false);
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
        PlaySoundFX(audioClips[2], false);
        process.MoveNext(Command.Follow);
    }

   

    void PickUp(GameObject targetObject)
    {
        if(this.carriedObject == null)
        {
            companionRenderer.material.color = Color.white;
            this.carriedObject = targetObject;
            EventsManager.instance.OnForceObjectBarrierEnableObstacle();
            targetObject.GetComponent<Rigidbody>().isKinematic = true;
            targetObject.GetComponent<NavMeshObstacle>().enabled = false;
            targetObject.transform.parent = gameObject.transform;
        }
    }

    void Drop(GameObject targetObject)
    {
        if(this.carriedObject != null)
        {
            this.carriedObject = null;
            this.isCarriedObjectFullyPickedUp = false;
            this.isCarriedObjectFullyRotated = false;
            EventsManager.instance.OnForceObjectBarrierDisableObstacle();
            targetObject.GetComponent<Rigidbody>().isKinematic = false;
            targetObject.transform.parent = null;
            targetObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * this.throwForce);
            targetObject.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    void Hack(GameObject targetGameObject)
    {
        PlaySoundFX(audioClips[6], false);
        companionRenderer.material.color = Color.black;
        this.hackedObject = targetGameObject;
        EventsManager.instance.OnCompanionHackEnable(this.hackedObject.GetInstanceID());
    }

    void StopHack(GameObject targetGameObject)
    {
        EventsManager.instance.OnCompanionHackDisable(targetGameObject.GetInstanceID());
        this.hackedObject = null;
    }

    private void PlaySoundFX(AudioClip ac, bool shouldLoop)
    {
        audioSrc.Stop();
        audioSrc.loop = shouldLoop;
        audioSrc.clip = ac;
        if (shouldLoop)
        {
            audioSrc.Play();
        } else
        {
            audioSrc.PlayOneShot(ac);
        }
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
            //throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return CurrentState;
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
