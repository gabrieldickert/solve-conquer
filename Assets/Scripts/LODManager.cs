using UnityEngine;

public class LODManager : MonoBehaviour
{
    public GameObject[] stages_LOD0;
    public GameObject[] stages_LOD1;
    public Vector3[] referencePointsDist;

    public float maxDistStage1;
    public float maxDistStage2;
    public float maxDistStage3;
    public float maxDistStage4;
    public float maxDistLandingPlatform;
    public float maxDistCrashedShip;

    private GameObject player;
    //private bool shouldWait = false;

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.instance.LODManagerEnable += OnLODManagerEnable;
        this.player = GameObject.FindGameObjectWithTag("Player");

        //halve the max distances to get the appropriate radius
        this.maxDistStage1 = this.maxDistStage1 / 2;
        this.maxDistStage2 = this.maxDistStage2 / 2;
        this.maxDistStage3 = this.maxDistStage3 / 2;
        this.maxDistStage4 = this.maxDistStage4 / 2;
        this.maxDistLandingPlatform = this.maxDistLandingPlatform / 2;
        this.maxDistCrashedShip = this.maxDistCrashedShip / 2;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStage(stages_LOD0[0], stages_LOD1[0], maxDistStage1, 1);
        UpdateStage(stages_LOD0[1], stages_LOD1[1], maxDistStage2, 2);
        UpdateStage(stages_LOD0[2], stages_LOD1[2], maxDistStage3, 3);
        UpdateStage(stages_LOD0[3], stages_LOD1[3], maxDistStage4, 4);
        UpdateStage(stages_LOD0[4], stages_LOD1[4], maxDistLandingPlatform, 5);
        UpdateStage(stages_LOD0[5], stages_LOD1[5], maxDistCrashedShip, 6);
    }

    void UpdateStage(GameObject lod0, GameObject lod1, float maxDistanceStage, int stage)
    {
        float distance = Vector3.Distance(player.transform.position, referencePointsDist[stage - 1]);
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
       
        stages_LOD1[stageNum - 1].SetActive(false);
        stages_LOD0[stageNum - 1].SetActive(true);
    }
}
