using System;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(this);
        }
    }

    public event Action<int> PressurePlateEnable;
    public event Action<int> PressurePlateDisable;
    public event Action<int> SwitchEnable;
    public event Action<int> SwitchDisable;
    public event Action<int> ResetObject;
    public event Action<int> ResetThrowable;
    public event Action ResetPlayer;
    public event Action ResetCompanion;
    public event Action<Vector3> CompanionWaitAt;
    public event Action<GameObject> CompanionPickUpObject;
    public event Action<GameObject> CompanionHackObject;
    public event Action CompanionFollow;
    public event Action<int> CompanionHackEnable;
    public event Action<int> CompanionHackDisable;
    public event Action ForceObjectBarrierEnableObstacle;
    public event Action ForceObjectBarrierDisableObstacle;
    public event Action CompanionDropObject;
    public event Action<int> LogicGateEnable;
    public event Action<int> LogicGateDisable;
    public event Action<int> PlayerEnteredTrigger;
    public event Action<int> ThrowableTargetEnable;
    public event Action<int> ThrowableTargetDisable;
    public event Action<int> AntennaEnable;
    public event Action<int> AntennaDisable;
    public event Action<int> LODManagerEnable;
    public event Action<int> PlayThrowSound;
    public event Action<int> AddTimelineToQueue;
    public event Action AudioManagerPlay;
    public event Action AudioManagerPause;
    public event Action AudioManagerVolumeDown;
    public event Action AudioManagerVolumeUp;

    private int companionId { get; set; }

    public void OnPressurePlateEnable(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateEnable");
        PressurePlateEnable?.Invoke(triggerId);
    }

    public void OnPressurePlateDisable(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateDisable");
        PressurePlateDisable?.Invoke(triggerId);
    }

    public void OnSwitchEnable(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateEnable");
        SwitchEnable?.Invoke(triggerId);
    }

    public void OnSwitchDisable(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateDisable");
        SwitchDisable?.Invoke(triggerId);
    }

    public void OnResetObject(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateDisable");
        ResetObject?.Invoke(instanceId);
    }
    public void OnResetThrowable(int instanceId)
    {
        ResetThrowable?.Invoke(instanceId);
    }
    public void OnResetPlayer()
    {

        ResetPlayer?.Invoke();
    }

    public void OnResetCompanion()
    {
        ResetCompanion?.Invoke();
    }

    public void OnCompanionWaitAt(Vector3 waitingPosition)
    {
        //UnityEngine.Debug.Log("EventsManager: OnCompanionWaitAt");
        CompanionWaitAt?.Invoke(waitingPosition);
    }

    public void OnCompanionPickUpObject(GameObject targetObject)
    {
        //UnityEngine.Debug.Log("EventsManager: OnCompanionPickUpObject");
        CompanionPickUpObject?.Invoke(targetObject);
    }

    public void OnCompanionHackObject(GameObject targetObject)
    {
        //UnityEngine.Debug.Log("EventsManager: OnCompanionHackObject");
        CompanionHackObject?.Invoke(targetObject);
    }

    public void OnCompanionFollow()
    {
        //UnityEngine.Debug.Log("EventsManager: OnCompanionFollow");
        CompanionFollow?.Invoke();
    }

    public void OnCompanionHackEnable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnCompanionHackEnable");
        CompanionHackEnable?.Invoke(instanceId);
    }

    public void OnCompanionHackDisable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnCompanionHackDisable");
        CompanionHackDisable?.Invoke(instanceId);
    }

    public void OnForceObjectBarrierEnableObstacle()
    {
        ForceObjectBarrierEnableObstacle?.Invoke();
    }

    public void OnForceObjectBarrierDisableObstacle()
    {
        ForceObjectBarrierDisableObstacle?.Invoke();
    }

    public void OnCompanionDropObject()
    {
        CompanionDropObject?.Invoke();
    }

    public void OnLogicGateEnable(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnLogicGateEnable");
        LogicGateEnable?.Invoke(triggerId);
    }

    public void OnLogicGateDisable(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnLogicGateDisable");
        LogicGateDisable?.Invoke(triggerId);
    }

    public void OnPlayerEnteredTrigger(int triggerId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPlayerEnteredTrigger");
        PlayerEnteredTrigger?.Invoke(triggerId);
    }

    public void OnThrowableTargetEnable(int triggerId)
    {
        ThrowableTargetEnable?.Invoke(triggerId);

    } 

    public void OnThrowableTargetDisable(int triggerId)
    {
        ThrowableTargetDisable?.Invoke(triggerId);
    }

    public void OnAntennaEnable(int triggerId)
    {
        AntennaEnable?.Invoke(triggerId);

    }

    public void OnAntennaDisable(int triggerId)
    {
        AntennaDisable?.Invoke(triggerId);
    }

    public void OnLODManagerEnable(int stageNum)
    {
        LODManagerEnable?.Invoke(stageNum);
    }

    public void OnPlayThrowSound(int objectId)
    {
        PlayThrowSound?.Invoke(objectId);
    }
    public void OnAddTimelineToQueue(int timelineID)
    {
        AddTimelineToQueue?.Invoke(timelineID);
    }

    public void OnAudioManagerPlay()
    {
        AudioManagerPlay?.Invoke();
    }

    public void OnAudioManagerPause()
    {
        AudioManagerPause?.Invoke();
    }

    public void OnAudioManagerVolumeDown()
    {
        AudioManagerVolumeDown?.Invoke();
    }

    public void OnAudioManagerVolumeUp()
    {
        AudioManagerVolumeUp?.Invoke();
    }
}