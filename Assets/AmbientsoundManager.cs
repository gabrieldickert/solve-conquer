using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientsoundManager : MonoBehaviour
{
    public static Sound ambientSound;
    public static AmbientsoundManager instance;
    public  bool playAmbientSound;
    public  float randomVolMin;
    public float randomVolMax;
    public  bool allowToSkip;
    public  float pitchMin;
    public  float pitchMax;
    public static bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {

        AmbientsoundManager.instance = this;

    }

    // Update is called once per frame
    void Update()
    {


        if(isActive)
        {
            if (ambientSound.source.isPlaying)
            {

                Debug.Log("abspielen");
            }

            else { Debug.Log("vorbei");
                AmbientsoundManager.SelectedNewAmbientClip();
                ambientSound.source.Play();
            }
        }
 



    }

    public static IEnumerator PlayAmbientSound()
    {
    
            
            ambientSound.source.Play();
            yield return new WaitForSeconds(ambientSound.source.clip.length);
            AmbientsoundManager.SelectedNewAmbientClip();


    }

    public static  void SelectedNewAmbientClip()
    {

        int randomIndex = Random.Range(0, ambientSound.clipList.Length);

        Debug.Log("RANDOM"+randomIndex);

        ambientSound.source.clip = ambientSound.clipList[randomIndex];
        ambientSound.source.volume = Random.Range(instance.randomVolMin,instance.randomVolMax);
        ambientSound.source.loop = false;

    }
    public static void StartPlayingAmbient(Sound s)
    {
      
        if(ambientSound == null)
        {

            ambientSound = s;
        }

        ambientSound.source.loop = false;
        isActive = true;
       // ambientSound.source.Play();

  



    }
}
