using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        gd = SaveSystem.LoadGame();

        string nextPlatformInfo = Regex.Split(this.name, "MovingPlatformTrigger_")[1];
        string nextPlatformStage = nextPlatformInfo[0].ToString();
        string nextPlatformLevel = nextPlatformInfo[1].ToString();

        //Saving current MovingPlatform
        this.Player = GameObject.FindWithTag("Player");
        this.Companion = GameObject.FindWithTag("Companion");
        this.Canvas = this.Player.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/IngameMessageCanvas").gameObject;
        this.Stage = nextPlatformStage;
        this.Lvl = nextPlatformLevel;
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
                    SkipLevelManager.currentIndex++;
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
