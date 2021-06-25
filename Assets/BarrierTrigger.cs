
using System.Collections;

using UnityEngine;

public class BarrierTrigger : MonoBehaviour
{
    public Barrier barrier;

    bool IsOpen = false;

    int CollionsObjCount = 0;
    public int TriggerId;

    Renderer PressurePlateRenderer;
    Vector3 initPos;
    float plateOffset;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Renderer component of the GameObject
        PressurePlateRenderer = GetComponent<Renderer>();
        PressurePlateRenderer.material.color = Color.red;
        initPos = gameObject.transform.position;
        //gameObject.transform.position = Vector3.MoveTowards(initPos, new Vector3(10f, 10f, 10f), 1f);
        //renderer = GetComponent<MeshRenderer>();
        plateOffset = 0.5f * PressurePlateRenderer.bounds.size.y;
    }
    // Update is called once per frame
    void Update()
    {
        {
            float step = 1f * Time.deltaTime; // calculate distance to move
            if (IsOpen)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(initPos.x, initPos.y - plateOffset, initPos.z), step);
                barrier.ChangeDoor(true);
                EventsManager.instance.OnPressurePlateEnable(TriggerId);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initPos, step);
                barrier.ChangeDoor(false);
                EventsManager.instance.OnPressurePlateDisable(TriggerId);
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
