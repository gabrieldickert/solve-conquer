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
        player = GameObject.FindWithTag("Player");
        boxCollider = gameObject.GetComponent<BoxCollider>();
    }
    void Update()
    {
        if(isFloor)
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
}
