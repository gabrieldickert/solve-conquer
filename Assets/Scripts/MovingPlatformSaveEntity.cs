using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformSaveEntity : MonoBehaviour
{

    private GameObject Player;
    private GameObject Companion;
    private GameObject Canvas;
    private string Stage;
    private string Lvl;
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
       
        GameData gd =   SaveSystem.LoadGame();

        if(!gd.MovingPlatformName.Equals(this.name))
        {
            Debug.Log(this.GetComponent<MovingPlatformNew>());
            SaveSystem.SaveGame(this.GetComponent<MovingPlatformNew>(), this.Stage, this.Lvl, this.Companion.activeInHierarchy ? true : false);
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
