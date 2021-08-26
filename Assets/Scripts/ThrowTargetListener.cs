using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTargetListener : MonoBehaviour
{

    public ThrowContainer ThrowContainer;
    public bool isActive = false;
    public int trigger1 = 0;
    public int trigger2 = 0;
   
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(!isActive)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(0, 255, 0, 1f);
            EventsManager.instance.OnThrowableTargetEnable(trigger1);
            EventsManager.instance.OnThrowableTargetEnable(trigger2);
      
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255, 0, 0, 1f);
            EventsManager.instance.OnThrowableTargetDisable(trigger1);
            EventsManager.instance.OnThrowableTargetDisable(trigger2);
        }
        
    }
}
