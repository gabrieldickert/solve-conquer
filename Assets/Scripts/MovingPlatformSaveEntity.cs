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
    private GameData gd;
    private MovingPlatformNew currentPlatform;
    private GameObject platform;


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

        gd = SaveSystem.LoadGame();
        platform = null;
        currentPlatform = null;

        if (gd != null)
        {
            platform = GameObject.Find(gd.MovingPlatformName);
            currentPlatform = platform.GetComponent<MovingPlatformNew>();
        }

        if (currentPlatform != null)
        {
            if (other.gameObject.tag == "Player")
            {
                currentPlatform.OnPlayerEnteredTrigger(other.gameObject.GetComponent<CapsuleCollider>());
            }

            if (other.gameObject.tag == "Companion")
            {
                currentPlatform.OnCompanionEnteredTrigger(other.gameObject.GetComponent<BoxCollider>());
            }
        }

        if (other.gameObject.tag == "Player")
        {
            if ((gd == null || !this.name.Equals(gd.MovingPlatformName)))
            {
                if ((currentPlatform != null && currentPlatform.hasRequiredPassengers) || gd == null)
                {
                    SaveSystem.SaveGame(this.GetComponent<MovingPlatformNew>(), this.Stage, this.Lvl, this.Companion.GetComponent<NavMeshAgent>().enabled ? true : false);
                    this.Canvas.SetActive(true);
                    StartCoroutine("WaitForSec");
                }

            }
        }

    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        this.Canvas.SetActive(false);
    }


}
