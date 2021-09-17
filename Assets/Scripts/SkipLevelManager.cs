using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SkipLevelManager : MonoBehaviour
{
    public static MovingPlatformSaveEntity[] movingPlatforms;
    public MovingPlatformSaveEntity[] allMovingPlatforms;
    public MovingPlatformSaveEntity currentPlatform;
    public MovingPlatformSaveEntity nextSavePlatform;

    public string level;
    public string stage;

    private static int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameData gd = SaveSystem.LoadGame();
        currentPlatform = GameObject.Find(gd.MovingPlatformName).GetComponent<MovingPlatformSaveEntity>();
        this.level = gd.lvl;
        this.stage = gd.stage;
        currentIndex = currentPlatform.order;
        this.nextSavePlatform = movingPlatforms[currentIndex + 1];
     
    }

    private void Update()
    {
        currentPlatform = movingPlatforms[currentIndex];
        
        if (currentIndex + 1 < movingPlatforms.Length)
        {
            nextSavePlatform = movingPlatforms[currentIndex + 1];
        } else if (currentIndex + 1 == movingPlatforms.Length)
        {
            nextSavePlatform = null;
        }
            
    }
    private void Awake()
    {
        movingPlatforms = FindObjectsOfType<MovingPlatformSaveEntity>().OrderBy(x => x.GetComponent<MovingPlatformSaveEntity>().order).ToArray();
        allMovingPlatforms = FindObjectsOfType<MovingPlatformSaveEntity>().OrderBy(x => x.GetComponent<MovingPlatformSaveEntity>().order).ToArray();
    }

    public static MovingPlatformSaveEntity SkipToNextLevel()
    {
        if(currentIndex + 1 < movingPlatforms.Length)
        {
            return movingPlatforms[++currentIndex];
        } else
        {
            return movingPlatforms[currentIndex];
        }
        
    }

}
