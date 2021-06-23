using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    bool doorStatus = false;

    Renderer BarrierRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ChangeDoor(bool isOpen)
    {
        if(!doorStatus && isOpen || doorStatus && !isOpen)
        {
            //Collision deaktivieren
            this.GetComponent<MeshCollider>().enabled = !isOpen;
            //Transparenz erhöhen
            //BarrierRenderer.material.color.a = isOpen ? 100 : 200;
            this.doorStatus = isOpen;
        }
        
        
    }

}
