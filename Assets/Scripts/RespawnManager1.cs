using System;
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
            // this.RespawnStageLevels.Add(stageobj.name, new List<GameObject>(GameObject.FindGameObjectsWithTag("Level")).FindAll(g => g.transform.IsChildOf(stageobj.transform)));
            //.Where(w=> w.transform.Find("Geometry")).ToList()
        }

        foreach (string key in this.RespawnStageLevels.Keys)
        {
            foreach (LevelInstance o in this.RespawnStageLevels[key])
            {
                for (int i = 0; i < o.levelobj.transform.Find("Geometry").childCount; i++)
                {
                    //   if(o.levelobj.name.Equals("Level2"))
                    //{
                    // Debug.Log(o.levelobj.transform.Find("Geometry").transform.GetChild(i).gameObject.name);
                    //Checking on Floor or not
                    o.geometryObjList.Add(o.levelobj.transform.Find("Geometry").transform.GetChild(i).gameObject);
                    // groupVec += o.levelobj.transform.Find("Geometry").transform.GetChild(i).gameObject.transform.position;
                    //  }
                }
               // Debug.Log(o.levelobj.name);
                Bounds b = CreateBound(o.geometryObjList.ToArray());
                GameObject go = new GameObject();
                go.transform.position = b.center;
                Vector3 respawncollidersize = new Vector3(b.size.x + 2 * b.extents.x, b.size.y + 2 * b.extents.y, b.size.z + 2 * b.extents.x);
                go.AddComponent<BoxCollider>().size = respawncollidersize;
                //Includes Y-Offset
                go.GetComponent<BoxCollider>().center = new Vector3(0, (b.max.y - b.min.y) * -1 - 5, 0);
                go.GetComponent<BoxCollider>().isTrigger = true;
                //Adding Respawnlistener to listen on Collision
                go.AddComponent<RespawnListener>();
            }
        }
    }

    private void Instance_ResetPlayer()
    {
        //Reset Player Position and Companion Pos (should be last save point)
        //Debug.Log("Reset Player");
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

    private Bounds CreateBound(GameObject[] gos)
    {
        if (gos.Length == 0)
            return new Bounds();
        if (gos.Length == 1)
            return new Bounds();
        Bounds bounds = new Bounds(gos[0].transform.position, Vector3.zero);
        for (var i = 1; i < gos.Length; i++)
            bounds.Encapsulate(gos[i].transform.position);
        return bounds;
    }
}

public class LevelInstance
{
    public string stage { get; set; }

    public GameObject levelobj { get; set; }

    public BoxCollider RespawnTrigger { get; set; }
    public List<GameObject> geometryObjList { get; set; }

    public List<RespawnObject> respawnObjList { get; set; }

    public LevelInstance(string stage, GameObject lvl)
    {
        this.stage = stage;
        this.levelobj = lvl;
        this.geometryObjList = new List<GameObject>();
        this.respawnObjList = new List<RespawnObject>();
        this.FindRespawnObjectsInLevel();
    }

    private void FindRespawnObjectsInLevel()
    {

 
        if(this.levelobj.transform.Find("Respawnables") != null)
        {
            for(int i = 0; i < this.levelobj.transform.Find("Respawnables").childCount;i++)
            {

                GameObject o = this.levelobj.transform.Find("Respawnables").GetChild(i).gameObject;

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
        }
    }
}