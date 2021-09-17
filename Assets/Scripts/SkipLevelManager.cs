using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipLevelManager : MonoBehaviour
{
    public static MovingPlatformSaveEntity[] movingPlatforms;
    public MovingPlatformSaveEntity[] allMovingPlatforms;
    public MovingPlatformSaveEntity currentPlatform;
    public GameObject nextSavePlatform;

    public string level;
    public string stage;

    public static int nextPlatform;
    private static int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameData gd = SaveSystem.LoadGame();
        currentPlatform = GameObject.Find(gd.MovingPlatformName).GetComponent<MovingPlatformSaveEntity>();
        this.level = gd.lvl;
        this.stage = gd.stage;

        
        foreach(MovingPlatformSaveEntity platform in movingPlatforms)
        {
           
            if(platform.name == currentPlatform.name)
            {
                nextPlatform = currentIndex + 1;
                nextSavePlatform = movingPlatforms[nextPlatform].gameObject;
            } else if (currentIndex < movingPlatforms.Length)
            {
                currentIndex++;
            } else if (currentIndex == movingPlatforms.Length)
            {
                nextSavePlatform = null;
            }
        }
    }

    private void Awake()
    {

        movingPlatforms = FindObjectsOfType<MovingPlatformSaveEntity>();
        allMovingPlatforms = FindObjectsOfType<MovingPlatformSaveEntity>();
    }

    public static GameObject skipToNextLevel()
    {
        if(nextPlatform != movingPlatforms.Length)
        {
            return movingPlatforms[nextPlatform].gameObject;
        } else
        {
            return movingPlatforms[currentIndex].gameObject; ;
        }
        
    }

}
