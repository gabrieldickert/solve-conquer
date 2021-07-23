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
    public event Action<Vector3> CompanionWaitAt;
    public event Action<GameObject> CompanionPickUpObject;
    public event Action<GameObject> CompanionHackObject;
    public event Action CompanionFollow;
    public event Action<int> HackableEnable;
    public event Action<int> HackableDisable;
    public event Action ReBake;

    public void OnPressurePlateEnable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateEnable");
        PressurePlateEnable?.Invoke(instanceId);
    }

    public void OnPressurePlateDisable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateDisable");
        PressurePlateDisable?.Invoke(instanceId);
    }

    public void OnSwitchEnable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateEnable");
        SwitchEnable?.Invoke(instanceId);
    }

    public void OnSwitchDisable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateDisable");
        SwitchDisable?.Invoke(instanceId);
    }

    public void OnResetObject(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnPressurePlateDisable");
        ResetObject?.Invoke(instanceId);
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

    public void OnHackableEnable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnHackableEnable");
        HackableEnable?.Invoke(instanceId);
    }

    public void OnHackableDisable(int instanceId)
    {
        //UnityEngine.Debug.Log("EventsManager: OnHackableDisable");
        HackableDisable?.Invoke(instanceId);
    }

    public void OnRebake()
    {
        ReBake?.Invoke();
    }
}