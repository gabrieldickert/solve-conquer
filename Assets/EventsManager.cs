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

    public void OnPressurePlateEnable(int instanceId)
    {
        PressurePlateEnable?.Invoke(instanceId);
    }

    public void OnPressurePlateDisable(int instanceId)
    {
        PressurePlateDisable?.Invoke(instanceId);
    }
}