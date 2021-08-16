using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnListener : MonoBehaviour
{
    public bool enablePlayerCollision;
    public bool enableCompanionCollision;
    public bool enableObjectCollision;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.gameObject.tag)
        {
            case "Player":
                if (enablePlayerCollision)
                {
                    EventsManager.instance.OnResetPlayer();
                }
                break;

            case "Companion":
                if (enableCompanionCollision)
                {
                    EventsManager.instance.OnResetCompanion();
                }

                break;
            case "GrabbableObject":
                if (enableObjectCollision)
                {
                    EventsManager.instance.OnResetObject(other.gameObject.GetInstanceID());
                }
                break;
        }
    }
}
