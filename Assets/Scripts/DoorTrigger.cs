using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    [SerializeField]
    GameObject Door;

    bool IsOpen = false;
    public float speed = 1.0f;

    int CollionsObjCount = 0;

    Renderer PressurePlateRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Renderer component of the GameObject
        PressurePlateRenderer = GetComponent<Renderer>();
        PressurePlateRenderer.material.color = Color.red;
    }
    // Update is called once per frame
    void Update()
    {
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            if (IsOpen)
            {

                Door.transform.position = Vector3.MoveTowards(Door.transform.position, new Vector3(Door.transform.position.x, 10, Door.transform.position.z), step);
            }
            else {
                Door.transform.position = Vector3.MoveTowards(Door.transform.position, new Vector3(Door.transform.position.x, 0, Door.transform.position.z), step);
            }


        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        CollionsObjCount++;
        IsOpen = true;
        PressurePlateRenderer.material.color = Color.green;
    }

    private void OnCollisionExit(Collision collision)
    {
        CollionsObjCount--;

        if(CollionsObjCount == 0)
        {
            IsOpen = false;
            PressurePlateRenderer.material.color = Color.red;
        }

     
    }

}
