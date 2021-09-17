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
    public int order;

    public static int nextPlatform;
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
     

        /*   foreach(MovingPlatformSaveEntity platform in movingPlatforms)
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
        */
    }

    private void Awake()
    {

        movingPlatforms = FindObjectsOfType<MovingPlatformSaveEntity>().OrderBy(x => x.GetComponent<MovingPlatformSaveEntity>().order).ToArray();


        allMovingPlatforms = FindObjectsOfType<MovingPlatformSaveEntity>().OrderBy(x => x.GetComponent<MovingPlatformSaveEntity>().order).ToArray();
    }

    public static GameObject SkipToNextLevel()
    {
        if(currentIndex < movingPlatforms.Length)
        {
            
            return movingPlatforms[++currentIndex].gameObject;
        } else
        {
            return movingPlatforms[currentIndex].gameObject;
        }
        
    }

}
