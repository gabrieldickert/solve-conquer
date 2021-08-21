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
        Debug.Log(AudioManager.currentTheme);
        if (FindObjectOfType<AudioManager>().Playing("Theme", AudioManager.currentTheme) && !AudioManager.currentThemePaused)
        {
            FindObjectOfType<AudioManager>().Pause("Theme", AudioManager.currentTheme);
            AudioManager.currentThemePaused = true;
        } else 
        {
            AudioManager.currentThemePaused = false;
            FindObjectOfType<AudioManager>().Play("Theme", AudioManager.currentTheme);
        }

    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.parent.parent.name == "Floor1" && !FindObjectOfType<AudioManager>().Playing("Theme", 0))
        {
            if (AudioManager.currentThemePaused)
            {
                AudioManager.currentTheme = 0;
                FindObjectOfType<AudioManager>().ChangeToClip("Theme", AudioManager.currentTheme);
            } else
            {
                AudioManager.currentTheme = 0;
                FindObjectOfType<AudioManager>().ChangeToClip("Theme", AudioManager.currentTheme);
                StartCoroutine(FindObjectOfType<AudioManager>().FadeIn("Theme", AudioManager.currentTheme, 3));
            }
        } else if(collision.transform.parent.parent.name == "Floor2" && !FindObjectOfType<AudioManager>().Playing("Theme", 1))
        {
            if (AudioManager.currentThemePaused)
            {
                AudioManager.currentTheme = 1;
                FindObjectOfType<AudioManager>().ChangeToClip("Theme", AudioManager.currentTheme);
            }
            else
            {
                AudioManager.currentTheme = 1;
                FindObjectOfType<AudioManager>().ChangeToClip("Theme", AudioManager.currentTheme);
                StartCoroutine(FindObjectOfType<AudioManager>().FadeIn("Theme", AudioManager.currentTheme, 3));
            }
        }
    }
    */
}
