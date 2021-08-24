using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWall : MonoBehaviour
{
    public bool isFloor = false;
    private GameObject player = null;
    private bool isColliderDisabled = false;
    private BoxCollider boxCollider = null;
    private float playerHeightOffset = 2f;
    private void Start()
    {
        //the script functionality is only needed, when a TeleportWall is used as floor,
        //hence the script disables itself otherwise to save performance
        if(isFloor)
        {
            player = GameObject.FindWithTag("Player");
            boxCollider = gameObject.GetComponent<BoxCollider>();
        } else
        {
            gameObject.GetComponent<TeleportWall>().enabled = false;
        }
    }
    void Update()
    {
        if (player.transform.position.y + playerHeightOffset >= gameObject.transform.position.y)
        {
            if(!isColliderDisabled)
            {
                boxCollider.enabled = false;
                isColliderDisabled = true;
            }
        }
        else if (isColliderDisabled)
        {
            boxCollider.enabled = true;
            isColliderDisabled = false;
        }
    }
}
