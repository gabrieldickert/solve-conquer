using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePad : MonoBehaviour
{
    public Player player;
    public Companion companion;
    public GameObject canvas;
    public bool saveCompanionPosition = false;

    void Start()
    {
        canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && SaveSystem.gameLoaded == false)
        {
            string stage = this.transform.parent.parent.parent.gameObject.name;
            string lvl = this.transform.parent.parent.gameObject.name;

            if(saveCompanionPosition)
            {
                SaveSystem.SaveGame(player, companion, this, stage, lvl, saveCompanionPosition);
            } else
            {
                SaveSystem.SaveGame(player, companion,this, stage, lvl, false);
            }
            
            canvas.SetActive(true);
            StartCoroutine("WaitForSec");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        SaveSystem.gameLoaded = false;
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        canvas.SetActive(false);
    }

}
