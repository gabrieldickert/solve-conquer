using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public int triggerId = 0;
    public bool triggerOnlyOnce = true;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Trigger hit");
            EventsManager.instance.OnPlayerEnteredTrigger(this.triggerId);
            if(triggerOnlyOnce)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
