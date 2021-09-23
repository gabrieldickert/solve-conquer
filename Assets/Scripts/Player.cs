using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public Companion companion;
    public GameObject menu;
    public GameObject vehicle;

    public GameObject canvas;

    private GameObject laserpointer;

    public PlayableDirector landingCutscene;

    public bool gameLoadedOnStart = false;

    private void Start()
    {
        this.canvas = this.transform.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor/IngameMessageCanvas").gameObject;

        GameData gd = SaveSystem.LoadGame();

        if ( gd != null)
        {
            this.LoadGame();
            gameLoadedOnStart = true;
        } else
        {
            landingCutscene.Play();
        }
        
    }


    public void LoadGame()
    {

        GameData gd = SaveSystem.LoadGame();

     if(gd != null)
        {
            if (gameLoadedOnStart)
            {
                SceneManager.LoadScene("PlanetScene", LoadSceneMode.Single);
            }


            
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.transform.parent = null;

            gameObject.transform.Find("LocomotionController").gameObject.SetActive(true);
            gameObject.transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").gameObject.GetComponent<LineRenderer>().enabled = true;
            gameObject.transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").gameObject.GetComponent<CompanionAimHandler>().enabled = true;
            gameObject.transform.rotation = UnityEngine.Quaternion.identity;
            gameObject.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

            //move spaceship to landing platform
            this.vehicle.transform.position = new UnityEngine.Vector3(333f, 56f, 644.9f);
            //rotate spaceship properly
            this.vehicle.transform.eulerAngles = new UnityEngine.Vector3(360f, 130f, 0f);

            MovingPlatformNew plat = GameObject.Find(gd.MovingPlatformName).GetComponent<MovingPlatformNew>();
            //Debug.Log(plat);
            GameObject player = GameObject.FindWithTag("Player");

            EventsManager.instance.OnLODManagerEnable(Int32.Parse("" + gd.stage.ToCharArray()[gd.stage.Length - 1]));

            //Debug.Log("Player: loaded stage " + Int32.Parse("" + gd.stage.ToCharArray()[gd.stage.Length - 1]));
           
            if (gd.stage.ToCharArray()[gd.stage.Length - 1] > '1' || (gd.lvl.ToCharArray()[gd.lvl.Length - 1] == '5' && gd.stage.ToCharArray()[gd.stage.Length - 1] == '1'))
            {
                GameObject companion = GameObject.FindWithTag("Companion");
                companion.GetComponent<NavMeshAgent>().enabled = false;
                companion.transform.parent = plat.VisualTrigger2.transform;
                companion.transform.position = plat.VisualTrigger2.GetComponent<MeshRenderer>().bounds.center;
                companion.GetComponent<NavMeshAgent>().enabled = true;
            }
            player.transform.parent = null;
            player.transform.parent = plat.VisualTrigger1.transform;
            player.transform.position = plat.VisualTrigger1.GetComponent<MeshRenderer>().bounds.center + new Vector3(0f,1f,0f);

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

    public void ExitGame() {

        Application.Quit();
    }
    
    
    public void StartNewGame()
    {

        string CurrenPath = Application.persistentDataPath + "/savegame.dat";

        if(File.Exists(CurrenPath))
        {
            try
            {
               File.Delete(CurrenPath);
               SceneManager.LoadScene("PlanetaryApproach", LoadSceneMode.Single);   
            }

            catch(Exception e)
            {
                Debug.LogException(e);

            }
        }

    }

    public void SkipLevel()
    {
     
        GameData gd = SaveSystem.LoadGame();
        
        if (gd != null)
        {
            MovingPlatformNew nextPlatform = SkipLevelManager.SkipToNextLevel().GetComponent<MovingPlatformNew>();
            
            string nextPlatformInfo = Regex.Split(nextPlatform.name, "MovingPlatformTrigger_")[1];
            string nextPlatformStage = nextPlatformInfo[0].ToString();
            string nextPlatformLevel = nextPlatformInfo[1].ToString();

            GameObject player = GameObject.FindWithTag("Player");

            if (!nextPlatform.name.Equals(gd.MovingPlatformName))
            {
                SaveSystem.SaveGame(nextPlatform, nextPlatformStage, nextPlatformLevel, GameObject.FindWithTag("Companion").GetComponent<NavMeshAgent>().enabled ? true : false);
                this.canvas.SetActive(true);
                StartCoroutine("WaitForSec");
                gd = SaveSystem.LoadGame();
            }
    
            EventsManager.instance.OnLODManagerEnable(Int32.Parse("" + gd.stage.ToCharArray()[gd.stage.Length - 1]));

            if (gd.stage.ToCharArray()[gd.stage.Length - 1] > '1' || (gd.lvl.ToCharArray()[gd.lvl.Length - 1] == '5' && gd.stage.ToCharArray()[gd.stage.Length - 1] == '1')) {
                GameObject companion = GameObject.FindWithTag("Companion");
                companion.GetComponent<NavMeshAgent>().enabled = false;
                companion.transform.parent = nextPlatform.VisualTrigger2.transform;
                companion.transform.position = nextPlatform.VisualTrigger2.GetComponent<MeshRenderer>().bounds.center;
                companion.GetComponent<NavMeshAgent>().enabled = true;
                StartCoroutine("WaitForSecCompanion", nextPlatform);
                
            }

            nextPlatform.GetComponent<BoxCollider>().enabled = false;
            player.transform.parent = nextPlatform.transform;
            player.transform.position = nextPlatform.VisualTrigger1.GetComponent<MeshRenderer>().bounds.center + new Vector3(0f, 1f, 0f);
            
            StartCoroutine("WaitForSecPlayer", nextPlatform);

        }

        menu.SetActive(false);
        IngameMenu.showMenu = false;
        laserpointer = GameObject.Find("LaserPointer");
        if (laserpointer != null)
        {
            laserpointer.SetActive(false);
        }
      
    }

    IEnumerator WaitForSecPlayer(MovingPlatformNew nextPlatform)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject player = GameObject.FindWithTag("Player");

        player.transform.parent = nextPlatform.transform;
        nextPlatform.GetComponent<BoxCollider>().enabled = true;

    }

    IEnumerator WaitForSecCompanion(MovingPlatformNew nextPlatform)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject companion = GameObject.FindWithTag("Companion");

        companion.transform.parent = nextPlatform.VisualTrigger2.transform;
      
    }

    IEnumerator WaitForSec()
    {
        yield return new WaitForSeconds(5);
        this.canvas.SetActive(false);
    }

}
