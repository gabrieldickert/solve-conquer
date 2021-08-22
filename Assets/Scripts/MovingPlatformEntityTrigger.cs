using UnityEngine;

public class MovingPlatformEntityTrigger : MonoBehaviour
{
    public MovingPlatformNew movingPlatform = null;
    public bool isCompanionTrigger = false;
    public bool isPlayerTrigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if(isCompanionTrigger && other.gameObject.tag == "Companion")
        {
            movingPlatform.OnCompanionEnteredTrigger(other);
        }
        if (isPlayerTrigger && other.gameObject.tag == "Player")
        {
            movingPlatform.OnPlayerEnteredTrigger(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isCompanionTrigger && other.gameObject.tag == "Companion")
        {
            movingPlatform.OnCompanionLeftTrigger(other);
        }
        if (isPlayerTrigger && other.gameObject.tag == "Player")
        {
            movingPlatform.OnPlayerLeftTrigger(other);
        }
    }
}
