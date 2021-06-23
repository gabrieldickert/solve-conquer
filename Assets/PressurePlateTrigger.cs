using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateTrigger : MonoBehaviour
{

    public Barrier barrier;
    bool IsOpen = false;
    bool HasFirstColObj = false;
    int CollionsObjCount = 0;

   public  GameObject LowerPressurePlate;
    Renderer PressurePlateRenderer;

    public float speed = 1f;

    public const float YOffset = 0.01f;


    Transform InitTrans = null;

    // Start is called before the first frame update
    void Start()
    {

        //Fetch the Renderer component of the GameObject
        PressurePlateRenderer = GetComponent<Renderer>();
        PressurePlateRenderer.material.color = Color.red;

        InitTrans = gameObject.transform;

    }

    // Update is called once per frame
    void Update()
    {



        {
            if (IsOpen)
            {

                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, 0.65f, gameObject.transform.position.z), speed * Time.deltaTime);
                //barrier.ChangeDoor(true);
            }
            else
            {

                if (HasFirstColObj) {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, 0.7f, gameObject.transform.position.z), speed * Time.deltaTime);
                }
          
             
 
                // barrier.ChangeDoor(false);
            }


        }

    }
    private void OnCollisionEnter(Collision collision)
    {

        HasFirstColObj = true;
        CollionsObjCount++;
        IsOpen = true;
        PressurePlateRenderer.material.color = Color.green;

      
    }

    private void OnCollisionExit(Collision collision)
    {
        CollionsObjCount--;

        if (CollionsObjCount == 0)
        {
            IsOpen = false;
            PressurePlateRenderer.material.color = Color.red;
        }


    }

}
