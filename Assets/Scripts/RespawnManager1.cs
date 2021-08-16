using System.Collections.Generic;
using UnityEngine;

public class RespawnManager1 : MonoBehaviour
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
        Debug.Log("Reset Player");
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
        if (this.levelobj.transform.Find("Respawn") != null)
        {
            for (int i = 0; i < this.levelobj.transform.Find("Respawn").transform.Find("Respawnables").childCount; i++)
            {
                GameObject o = this.levelobj.transform.Find("Respawn").transform.Find("Respawnables").GetChild(i).gameObject;

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