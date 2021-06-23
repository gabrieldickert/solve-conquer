using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrierTrigger : MonoBehaviour
{

    
    GameObject Door;
    public Barrier barrier;

    bool IsOpen = false;

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
            if (IsOpen)
            {

                barrier.ChangeDoor(true);
            }
            else
            {
                barrier.ChangeDoor(false);
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

        if (CollionsObjCount == 0)
        {
            IsOpen = false;
            PressurePlateRenderer.material.color = Color.red;
        }


    }

}
