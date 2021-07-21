using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;

public class TelekinesisHand : MonoBehaviour
{

    public LayerMask AimCollisionLayerMask;

    //render our hand pointer raycast
    public LineRenderer telekinesisLine;

    //information about the line render
    public float lineWidth = 0.01f;
    public float lineMaxLength = 1f;

    //bool to determine if the line reder is enabled or disabled
    public bool toggled = false;

    //store input from our left hand trigger
    private float HandLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);

    //bool to determine if we hit an enemy with the raycast
    public bool enemyHitLeft = false;

    private int layer_mask;

    //gameobject to store the enemy
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //set up our line rederer
        Vector3[] startLinePositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        telekinesisLine.SetPositions(startLinePositions);
        telekinesisLine.enabled = false;
        telekinesisLine.startWidth = lineWidth;
        telekinesisLine.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        //update the value of HandLeft every frame with new value from trigger
        HandLeft = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);

        //turn on/off the line renderer if trigger is pulled in
        if (HandLeft > 0.9)
        {
            toggled = true;
            telekinesisLine.enabled = true;
        } else
        {
            telekinesisLine.enabled = false;
            toggled = false;
            //make sure that we cant register hit on an enemy when the line renderer is turned off
            enemyHitLeft = false;
        }

        if (toggled)
        {
            //starts the raycast if we have pulled the trigger
            Telekinesis(transform.position, transform.forward, lineMaxLength);
        }
    }


    private void Telekinesis(Vector3 targetPosition, Vector3 direction, float length)
    {
        //set up raycast hit
        RaycastHit hit;

        //set up raycast
        Ray telekinesisOut = new Ray(targetPosition, direction);

        //declares an end position variable for the line renderer
        Vector3 endPosition = targetPosition + (length * direction);

        //run the raycast
        if (Physics.Raycast(telekinesisOut, out hit, lineMaxLength, AimCollisionLayerMask))
        {
            //update the line render with the new end position
            endPosition = hit.point;

            EventsManager.instance.OnCompanionWaitAt(endPosition);

            //set the enemy game object to the gameobject that the raycast hit
            enemy = hit.collider.gameObject;

            //if enemy has the telekinesisExplode script
            /*
            if (enemy.GetComponent<telekinesisExplode>())
            {
                enemyHitLeft = true;
                //update bool in telekinesisExplode script
                enemy.GetComponent<telekinesisExplode>().leftHandRay = true;
                //debugging
                Debug.Log("EnemyHit value is: " + enemyHitLeft);
            } else
            {*/
                enemyHitLeft = false;
                //debugging
                Debug.Log("EnemyHit value is: " + enemyHitLeft);
            /*}*/
            //if the raycast stops set enemyHitLeft to false
        } else if (enemyHitLeft)
        {
            enemyHitLeft = false;
            //debugging
            Debug.Log("EnemyHit value is: " + enemyHitLeft);
        }
        //update our line renderer declared at top of file
        telekinesisLine.SetPosition(0, targetPosition);
        telekinesisLine.SetPosition(1, endPosition);
    }

}
