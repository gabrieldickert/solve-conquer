using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class RespawnManager : MonoBehaviour
{
    public Dictionary<string, List<LevelInstance>> RespawnStageLevels = new Dictionary<string, List<LevelInstance>>();
    public GameObject PlayerController;
    public GameObject Companion;

    // Start is called before the first frame update
    private void Start()
    {
        EventsManager.instance.ResetCompanion += Instance_ResetCompanion;
        EventsManager.instance.ResetPlayer += Instance_ResetPlayer;
        
        //Loop through every Stage, find every level and create a List containg the Levelobjects
        foreach (GameObject stageobj in GameObject.FindGameObjectsWithTag("Stage"))
        {
            List<GameObject> levelobjs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Level")).FindAll(g => g.transform.IsChildOf(stageobj.transform));
            this.RespawnStageLevels.Add(stageobj.name, new List<LevelInstance>());

            for (int i = 0; i < levelobjs.Count; i++)
            {
                this.RespawnStageLevels[stageobj.name].Add(new LevelInstance(stageobj.name, levelobjs[i]));
            }
        }
    }

    private void Instance_ResetPlayer()
    {
        //Reset Player Position and Companion Pos (should be last save point)
        //Debug.Log("Reset Player");
        SceneManager.LoadScene("PlanetScene", LoadSceneMode.Single);
        
        GameData gd = SaveSystem.LoadGame();

        Debug.Log(gd.MovingPlatformName);

        MovingPlatformNew plat = GameObject.Find(gd.MovingPlatformName).GetComponent<MovingPlatformNew>();

        //Create Playable State for Player Component
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.transform.parent = null;
        player.transform.Find("LocomotionController").gameObject.SetActive(true);
        player.transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").gameObject.GetComponent<LineRenderer>().enabled = true;
        player.transform.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").gameObject.GetComponent<CompanionAimHandler>().enabled = true;
        player.transform.rotation = UnityEngine.Quaternion.identity;
        player.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

        player.transform.parent = plat.VisualTrigger1.transform;
        player.transform.position = plat.VisualTrigger1.GetComponent<MeshRenderer>().bounds.center;

        if(gd.saveCompanionPosition)
        {
            GameObject companion = GameObject.FindWithTag("Companion");
            companion.GetComponent<NavMeshAgent>().enabled = false;
            companion.transform.parent = plat.VisualTrigger2.transform;
            companion.transform.position = plat.VisualTrigger2.GetComponent<MeshRenderer>().bounds.center;
            companion.GetComponent<NavMeshAgent>().enabled = true;
        }
        
        /*
        Vector3 spawnPos = new Vector3(gd.positionPad[0], gd.positionPad[1], gd.positionPad[2]);
        this.PlayerController.transform.position = spawnPos;

        //FindObjectOfType<AudioManager>().Play("PlayerDeath", 0);

        if (gd.saveCompanionPosition)
        {
            this.Companion.transform.GetComponent<NavMeshAgent>().enabled = false;
            spawnPos.x += 1.5f;
            this.Companion.transform.position = spawnPos;
            this.Companion.transform.GetComponent<NavMeshAgent>().enabled = true;
        }
        */
        LevelInstance currentLvl = this.RespawnStageLevels[gd.stage].Find(g => g.levelobj.name.Equals(gd.lvl));

        foreach (RespawnObject o in currentLvl.respawnObjList)
        {
            o.gameObject.transform.position = o.initialPosition;
        }
    }

    private void Instance_ResetCompanion()
    {
        //Reset Player Position and Companion Pos (should be last save point)
        //Debug.Log("Reset Companion");
        this.Instance_ResetPlayer();
    }

    
}

public class LevelInstance
{
    public string stage { get; set; }
    public GameObject levelobj { get; set; }
    public BoxCollider RespawnTrigger { get; set; }
    public List<RespawnObject> respawnObjList { get; set; }

    public LevelInstance(string stage, GameObject lvl)
    {
        this.stage = stage;
        this.levelobj = lvl;
        this.respawnObjList = new List<RespawnObject>();
        this.FindRespawnObjectsInLevel();
    }

    private void FindRespawnObjectsInLevel()
    {
        List<GameObject> floorList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Floor")).FindAll(g => g.transform.IsChildOf(this.levelobj.transform));


   
        foreach (GameObject floor in floorList)
        {
            for (int i = 0; i < floor.transform.Find("Respawn").transform.Find("Respawnables").childCount; i++)
            {
                GameObject o = floor.transform.Find("Respawn").transform.Find("Respawnables").GetChild(i).gameObject;

                respawnObjList.Add(new RespawnObject(o));
            }
        }
        //resort all respawnables to avoid bugs with grabbable objects
        //ReorderRespawnablesInSceneTree();

    }

    private void ReorderRespawnablesInSceneTree()
    {
        foreach(RespawnObject o in respawnObjList)
        {
            o.gameObject.transform.parent = null;
        }
    }
}

public class RespawnObject
{
    public GameObject gameObject { get; set; }
    public Vector3 initialPosition { get; set; }

    public RespawnObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.initialPosition = this.gameObject.transform.position;
        EventsManager.instance.ResetObject += HandleResetObject;
    }

    public void HandleResetObject(int instanceid)
    {
        if (this.gameObject.GetInstanceID() == instanceid)
        {
            this.gameObject.transform.position = initialPosition;
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}