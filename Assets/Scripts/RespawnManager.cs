using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RespawnManager : MonoBehaviour
{
    public Dictionary<string, List<LevelInstance>> RespawnStageLevels = new Dictionary<string, List<LevelInstance>>();
    public GameObject PlayerController;
    public GameObject Companion;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("ok");
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
        Debug.Log("Reset Player");

        GameData gd = SaveSystem.LoadGame();
        Vector3 spawnPos = new Vector3(gd.positionPad[0], gd.positionPad[1], gd.positionPad[2]);
        this.PlayerController.transform.position = spawnPos;
        if (gd.saveCompanionPosition)
        {
            this.Companion.transform.GetComponent<NavMeshAgent>().enabled = false;
            spawnPos.x += 1.5f;
            this.Companion.transform.position = spawnPos;
            this.Companion.transform.GetComponent<NavMeshAgent>().enabled = true;
        }
      
        LevelInstance currentLvl = this.RespawnStageLevels[gd.stage].Find(g => g.levelobj.name.Equals(gd.lvl));

        foreach (RespawnObject o in currentLvl.respawnObjList)
        {
            o.gameObject.transform.position = o.initialPosition;
        }
    }

    private void Instance_ResetCompanion()
    {
        //Reset Player Position and Companion Pos (should be last save point)
        Debug.Log("Reset Companion");
    }

    // Update is called once per frame
    private void Update()
    {
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