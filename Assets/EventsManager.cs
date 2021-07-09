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
}