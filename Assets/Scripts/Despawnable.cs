using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawnable : MonoBehaviour
{
    public bool despawnAfterCollision = false;
    public float despawnAfterSeconds = 10f;
    private float startTime = 0f;
    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float timePassed = Time.time - startTime;
        
        if(!despawnAfterCollision || (despawnAfterCollision && hasCollided))
        {
            if(timePassed > despawnAfterSeconds)
            {
                //Debug.Log("Despawnable: Despawning object");
                Destroy(gameObject);
            }
                
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Despawnable")
        {
            if (!hasCollided)
            {
                hasCollided = true;
                startTime = Time.time;
                //Debug.Log("Despawnable: Object collided, despawning in " + despawnAfterSeconds + " seconds");
            }
        }
        
    }
}
