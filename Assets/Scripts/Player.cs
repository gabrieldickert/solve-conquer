using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour
{
    public Companion companion;
    public GameObject menu;
    public GameObject vehicle;
    public PlayableDirector landingCutscene;

    public bool gameLoadedOnStart = false;
    /*private SaveGamePad pad;
    private int currentStage = 0;
    private int currentLevel = 0;*/

    private void Start()
    {
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
            
            GameObject player = GameObject.FindWithTag("Player");

            
            EventsManager.instance.OnLODManagerEnable(Int32.Parse("" + gd.stage.ToCharArray()[gd.stage.Length - 1]));
            if (gd.stage.ToCharArray()[gd.stage.Length - 1] > '1' || (gd.lvl.ToCharArray()[gd.lvl.Length - 1] == '4' && gd.stage.ToCharArray()[gd.stage.Length - 1] == '1') || (gd.lvl.ToCharArray()[gd.lvl.Length - 1] == '5' && gd.stage.ToCharArray()[gd.stage.Length - 1] == '1')) {
                GameObject companion = GameObject.FindWithTag("Companion");
                companion.GetComponent<NavMeshAgent>().enabled = false;
                companion.transform.parent = nextPlatform.VisualTrigger2.transform;
                companion.transform.position = nextPlatform.VisualTrigger2.GetComponent<MeshRenderer>().bounds.center;
                companion.GetComponent<NavMeshAgent>().enabled = true;
                StartCoroutine("WaitForSecCompanion", nextPlatform);
                
            }

            player.transform.parent = nextPlatform.transform;
            player.transform.position = nextPlatform.VisualTrigger1.GetComponent<MeshRenderer>().bounds.center + new Vector3(0f, 1f, 0f);
            
            StartCoroutine("WaitForSecPlayer", nextPlatform);

        }

        menu.SetActive(false);
        IngameMenu.showMenu = false;

    }

    IEnumerator WaitForSecPlayer(MovingPlatformNew nextPlatform)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject player = GameObject.FindWithTag("Player");

        player.transform.parent = nextPlatform.transform;
        
    }

    IEnumerator WaitForSecCompanion(MovingPlatformNew nextPlatform)
    {
        yield return new WaitForSeconds(0.1f);

        GameObject companion = GameObject.FindWithTag("Companion");

        companion.transform.parent = nextPlatform.VisualTrigger2.transform;
      
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
