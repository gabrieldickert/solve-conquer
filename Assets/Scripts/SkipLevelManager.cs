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

    public string stage;
    public string level;
    
    public static int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameData gd = SaveSystem.LoadGame();
        if(gd != null)
        {
            currentPlatform = GameObject.Find(gd.MovingPlatformName).GetComponent<MovingPlatformSaveEntity>();
            this.stage = gd.stage;
            this.level = gd.lvl;
            currentIndex = currentPlatform.order;

            if (currentIndex != movingPlatforms.Length - 1)
            {
                this.nextSavePlatform = movingPlatforms[currentIndex + 1];
            }
            else
            {
                this.nextSavePlatform = movingPlatforms[currentIndex];
            }
        }
        
    }

    private void Update()
    {
        if (currentIndex + 1 < movingPlatforms.Length)
        {
            currentPlatform = movingPlatforms[currentIndex];
            stage = movingPlatforms[currentIndex].Stage;
            level = movingPlatforms[currentIndex].Lvl;
        }
        
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
