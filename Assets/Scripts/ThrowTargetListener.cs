using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTargetListener : MonoBehaviour
{

    public bool isActive = false;
    public int trigger1 = 0;
    public int trigger2 = 0;

    public MeshRenderer lightRenderer1 = null;
    public MeshRenderer lightRenderer2 = null;

    // Start is called before the first frame update
    void Start()
    {
        lightRenderer1.material.color = Color.red;
        lightRenderer2.material.color = Color.red;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Throwable")
        {
            if (!isActive)
            {
                lightRenderer1.material.color = Color.green;
                lightRenderer2.material.color = Color.green;
                EventsManager.instance.OnThrowableTargetEnable(trigger1);
                EventsManager.instance.OnThrowableTargetEnable(trigger2);
                isActive = true;}
            else if (isActive)
            {
                lightRenderer1.material.color = Color.red;
                lightRenderer2.material.color = Color.red;
                EventsManager.instance.OnThrowableTargetDisable(trigger1);
                EventsManager.instance.OnThrowableTargetDisable(trigger2);
                isActive = false;
            }
        }
    }
}
