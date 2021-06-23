using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //Invertierte Barriere, zB. Brücke
    public bool isBridge = false;

    bool barrierStatus = false;
    

    Renderer BarrierRenderer;

    // Start is called before the first frame update
    void Start()
    {
        if(isBridge)
        {
            barrierStatus = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ChangeDoor(bool isOpen)
    {
        
        if(!barrierStatus && isOpen || barrierStatus && !isOpen)
        {
           if(isBridge)
            {
                //Collision deaktivieren
                this.GetComponent<MeshCollider>().enabled = isOpen;
                //Transparenz erhöhen
                //BarrierRenderer.material.color.a = isOpen ? 100 : 200;   
            } else
            {
                //Collision deaktivieren
                this.GetComponent<MeshCollider>().enabled = !isOpen;
                //Transparenz erhöhen
                //BarrierRenderer.material.color.a = isOpen ? 100 : 200;
            }
            this.barrierStatus = isOpen;
        }
        
        
    }

}
