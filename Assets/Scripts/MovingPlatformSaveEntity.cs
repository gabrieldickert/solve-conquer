using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingPlatformSaveEntity : MonoBehaviour
{

    private GameObject Player;
    private GameObject Companion;
    private GameObject Canvas;

    [HideInInspector] public string Stage;
    [HideInInspector] public string Lvl;

    public int order;

    // Start is called before the first frame update
    void Start()
    {
        //Saving current MovingPlatform
        this.Player = GameObject.FindWithTag("Player");
        this.Companion = GameObject.FindWithTag("Companion");
        this.Canvas = this.Player.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/IngameMessageCanvas").gameObject;
        this.Stage = this.transform.parent.parent.parent.parent.gameObject.name;
        this.Lvl = this.transform.parent.parent.parent.gameObject.name;
        this.Canvas.SetActive(false);

    }

    private void OnTriggerEnter(Collider other)
    {
       
        GameData gd = SaveSystem.LoadGame();
        GameObject platform = GameObject.Find(gd.MovingPlatformName);
        MovingPlatformNew omegalul = platform.GetComponent<MovingPlatformNew>();

        if(other.gameObject.tag == "Player")
        {
            omegalul.OnPlayerEnteredTrigger(other.gameObject.GetComponent<CapsuleCollider>());
        }

        if(other.gameObject.tag == "Companion")
        {
            omegalul.OnCompanionEnteredTrigger(other.gameObject.GetComponent<BoxCollider>());

        }

        if((gd == null || !this.name.Equals(gd.MovingPlatformName)) && omegalul.hasRequiredPassengers)
        {
                SaveSystem.SaveGame(this.GetComponent<MovingPlatformNew>(), this.Stage, this.Lvl, this.Companion.GetComponent<NavMeshAgent>().enabled ? true : false);
                this.Canvas.SetActive(true);
                StartCoroutine("WaitForSec");
        } 

    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        this.Canvas.SetActive(false);


    }

}
