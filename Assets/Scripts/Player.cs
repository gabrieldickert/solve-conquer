using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public Companion companion;
    public GameObject menu;

    private SaveGamePad pad;

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        //Vector3 positionPlayer;
        //positionPlayer.x = data.positionPlayer[0];
        //positionPlayer.y = data.positionPlayer[1];
        //positionPlayer.z = data.positionPlayer[2];
        //this.transform.position = positionPlayer;

        //Vector3 positionCompanion;
        //positionCompanion.x = data.positionCompanion[0];
        //positionCompanion.y = data.positionCompanion[1];
        //positionCompanion.z = data.positionCompanion[2];
        //companion.transform.position = positionCompanion;

        Vector3 positionPad;
        positionPad.x = data.positionPad[0];
        positionPad.y = data.positionPad[1];
        positionPad.z = data.positionPad[2];

        this.transform.position = positionPad;
        Debug.Log(data.saveCompanionPosition);
        if(data.saveCompanionPosition)
        {
            companion.GetComponent<NavMeshAgent>().enabled = false;
            companion.transform.position = positionPad + new Vector3(1.5f, 0, 0);
            companion.GetComponent<NavMeshAgent>().enabled = true;
        }
  
        menu.SetActive(false);
        IngameMenu.showMenu = false;
    }

    public void PauseTheme()
    {
        if (FindObjectOfType<AudioManager>().Playing("Theme"))
        {
            FindObjectOfType<AudioManager>().Pause("Theme");
        } else
        {
            FindObjectOfType<AudioManager>().Play("Theme");
        }
    }
}
