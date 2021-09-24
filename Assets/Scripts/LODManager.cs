using UnityEngine;

public class LODManager : MonoBehaviour
{
    public GameObject[] stages_LOD0;
    public GameObject[] stages_LOD1;
    public Vector3[] referencePointsDist;

    public float maxDistStage1;
    public float maxDistStage2;
    //public float maxDistStage3;

    public float maxDistStage3_L1;
    public float maxDistStage3_L2;
    public float maxDistStage3_L3;
    public float maxDistStage3_L4;
    public float maxDistStage3_L5;

    //public float maxDistStage4;
    public float maxDistStage4_L1;
    public float maxDistStage4_L2;
    public float maxDistStage4_L3;
    public float maxDistStage4_L4;

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

        //this.maxDistStage3 = this.maxDistStage3 / 2;
        this.maxDistStage3_L1 = this.maxDistStage3_L1 / 2;
        this.maxDistStage3_L2 = this.maxDistStage3_L2 / 2;
        this.maxDistStage3_L3 = this.maxDistStage3_L3 / 2;
        this.maxDistStage3_L4 = this.maxDistStage3_L4 / 2;
        this.maxDistStage3_L5 = this.maxDistStage3_L5 / 2;

        //this.maxDistStage4 = this.maxDistStage4 / 2;
        this.maxDistStage4_L1 = this.maxDistStage4_L1 / 2;
        this.maxDistStage4_L2 = this.maxDistStage4_L2 / 2;
        this.maxDistStage4_L3 = this.maxDistStage4_L3 / 2;
        this.maxDistStage4_L4 = this.maxDistStage4_L4 / 2;

        this.maxDistLandingPlatform = this.maxDistLandingPlatform / 2;
        this.maxDistCrashedShip = this.maxDistCrashedShip / 2;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStage(stages_LOD0[0], stages_LOD1[0], maxDistStage1, 0);
        UpdateStage(stages_LOD0[1], stages_LOD1[1], maxDistStage2, 1);

        //UpdateStage(stages_LOD0[2], stages_LOD1[2], maxDistStage3, 3);

        UpdateStage(stages_LOD0[2], stages_LOD1[2], maxDistStage3_L1, 2);
        UpdateStage(stages_LOD0[3], stages_LOD1[3], maxDistStage3_L2, 3);
        UpdateStage(stages_LOD0[4], stages_LOD1[4], maxDistStage3_L3, 4);
        UpdateStage(stages_LOD0[5], stages_LOD1[5], maxDistStage3_L4, 5);
        UpdateStage(stages_LOD0[6], stages_LOD1[6], maxDistStage3_L5, 6);

        //UpdateStage(stages_LOD0[7], stages_LOD1[7], maxDistStage4, 7);
        UpdateStage(stages_LOD0[7], stages_LOD1[7], maxDistStage4_L1, 7);
        UpdateStage(stages_LOD0[8], stages_LOD1[8], maxDistStage4_L2, 8);
        UpdateStage(stages_LOD0[9], stages_LOD1[9], maxDistStage4_L3, 9);
        UpdateStage(stages_LOD0[10], stages_LOD1[10], maxDistStage4_L4, 10);

        UpdateStage(stages_LOD0[11], stages_LOD1[11], maxDistLandingPlatform, 11);
        UpdateStage(stages_LOD0[12], stages_LOD1[12], maxDistCrashedShip, 12);
    }

    void UpdateStage(GameObject lod0, GameObject lod1, float maxDistanceStage, int index)
    {
        float distance = Vector3.Distance(player.transform.position, referencePointsDist[index]);
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
        if (stageNum == 1 || stageNum == 2)
        {
            stages_LOD1[stageNum - 1].SetActive(false);
            stages_LOD0[stageNum - 1].SetActive(true);
        } else if (stageNum == 3)
        {
            for(int i = 2; i <= 6; i++)
            {
                stages_LOD1[i].SetActive(false);
                stages_LOD0[i].SetActive(true);
            }
        } else if (stageNum == 4)
        {
            for (int i = 7; i <= 10; i++)
            {
                stages_LOD1[i].SetActive(false);
                stages_LOD0[i].SetActive(true);
            }
        }
        
    }
}
