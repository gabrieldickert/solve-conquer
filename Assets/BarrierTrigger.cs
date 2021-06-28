
using System.Collections;

using UnityEngine;

public class BarrierTrigger : MonoBehaviour
{
    bool isOpen = false;

    int collionsObjCount = 0;
    public int triggerId;

    Renderer pressurePlateRenderer;
    Vector3 initPos;
    float plateOffset;

    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Renderer component of the GameObject
        pressurePlateRenderer = GetComponent<Renderer>();
        pressurePlateRenderer.material.color = Color.red;
        initPos = gameObject.transform.position;
        plateOffset = 0.5f * pressurePlateRenderer.bounds.size.y;
    }
    // Update is called once per frame
    void Update()
    {
        {
            float step = 1f * Time.deltaTime; // calculate distance to move
            if (isOpen)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(initPos.x, initPos.y - plateOffset, initPos.z), step);
            }
            else
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, initPos, step);
            }


        }

    }


    private void OnCollisionEnter(Collision collision)
    {
        collionsObjCount++;
        isOpen = true;
        pressurePlateRenderer.material.color = Color.green;
        EventsManager.instance.OnPressurePlateEnable(triggerId);
    }

    private void OnCollisionExit(Collision collision)
    {
        collionsObjCount--;

        if (collionsObjCount == 0)
        {
            isOpen = false;
            pressurePlateRenderer.material.color = Color.red;
            EventsManager.instance.OnPressurePlateDisable(triggerId);
        }


    }

}
