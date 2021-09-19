using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LODManager : MonoBehaviour
{
    public GameObject Stage1_LOD0;
    public GameObject Stage1_LOD1;

    public GameObject Stage2_LOD0;
    public GameObject Stage2_LOD1;

    public GameObject Stage3_LOD0;
    public GameObject Stage3_LOD1;

    public GameObject Stage4_LOD0;
    public GameObject Stage4_LOD1;

    public float maxDistStage1;
    public float maxDistStage2;
    public float maxDistStage3;
    public float maxDistStage4;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStage(Stage1_LOD0, Stage1_LOD1, maxDistStage1);
        UpdateStage(Stage2_LOD0, Stage2_LOD1, maxDistStage2);
        UpdateStage(Stage3_LOD0, Stage3_LOD1, maxDistStage3);
        UpdateStage(Stage4_LOD0, Stage4_LOD1, maxDistStage4);
    }

    void UpdateStage(GameObject lod0, GameObject lod1, float maxDistanceStage)
    {
        float distance = Vector3.Distance(player.transform.position, lod0.transform.position);

        if (distance < maxDistanceStage)
        {
            //disable LOD0
            lod0.SetActive(false);
            //enable LOD1
            lod1.SetActive(true);
        }
        else
        {
            //disable LOD1
            lod1.SetActive(false);
            //enable LOD0
            lod0.SetActive(true);
        }
    }
}
