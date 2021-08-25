using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowContainer : MonoBehaviour
{

    private Dictionary<GameObject,Vector3> ThrowablesDic;
    public bool AllowRespawn = true;
    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.ResetThrowable += ResetThrowableObject;
        //Check for all Throwables and get initpos
        this.ThrowablesDic = new Dictionary<GameObject, Vector3>();

        Transform t = this.gameObject.transform.Find("Throwables");
     
        for(int i = 0; i < t.childCount;i++)
        {

            ThrowablesDic.Add(t.GetChild(i).gameObject, t.GetChild(i).position);

         
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetThrowableObject(int instanceID) {

    if(AllowRespawn)
        {
            foreach (GameObject o in this.ThrowablesDic.Keys)
            {
                if (o.GetInstanceID() == instanceID)
                {
                    o.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                    o.transform.position = this.ThrowablesDic[o];

                }
            }
        }

    
    }
}
