using UnityEngine;

public class LODManager : MonoBehaviour
{
    public GameObject[] stages_LOD0;
    public GameObject[] stages_LOD1;

    public float maxDistStage1;
    public float maxDistStage2;
    public float maxDistStage3;
    public float maxDistStage4;

    private GameObject player;
    //private bool shouldWait = false;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.LODManagerEnable += OnLODManagerEnable;
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStage(stages_LOD0[0], stages_LOD1[0], maxDistStage1);
        UpdateStage(stages_LOD0[1], stages_LOD1[1], maxDistStage2);
        UpdateStage(stages_LOD0[2], stages_LOD1[2], maxDistStage3);
        UpdateStage(stages_LOD0[3], stages_LOD1[3], maxDistStage4);
    }

    void UpdateStage(GameObject lod0, GameObject lod1, float maxDistanceStage)
    {
        float distance = Vector3.Distance(player.transform.position, lod0.transform.position);
        //Debug.Log("LODManager: distance to stage " + lod0 + " = " + distance);
        if (distance <= maxDistanceStage)
        {
            //enable LOD0
            lod0.SetActive(true);
            //disable LOD1
            lod1.SetActive(false);
        }
        else
        {
            //enable LOD1
            lod1.SetActive(true);
            //disable LOD0
            lod0.SetActive(false);
        }   
    }

    void OnLODManagerEnable(int stageNum)
    {
        //this.shouldWait = true;
        stages_LOD1[stageNum].SetActive(false);
        stages_LOD0[stageNum].SetActive(true);
    }
}
