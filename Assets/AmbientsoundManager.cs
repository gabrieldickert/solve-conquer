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
    // Start is called before the first frame update
    void Start()
    {

        AmbientsoundManager.instance = this;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static IEnumerator PlayAmbientSound()
    {
    
        while(instance.playAmbientSound)
        {
            ambientSound.source.Play();
            yield return new WaitForSeconds(ambientSound.source.clip.length);
            AmbientsoundManager.SelectedNewAmbientClip();
            ambientSound.source.Play();

        }


    }

    public static  void SelectedNewAmbientClip()
    {

        int randomIndex = Random.Range(0, ambientSound.clipList.Length);

        ambientSound.source.clip = ambientSound.clipList[randomIndex];
        ambientSound.source.volume = Random.Range(instance.randomVolMin,instance.randomVolMax);

    }
    public static void StartPlayingAmbient(Sound s)
    {
      
        if(ambientSound == null)
        {

            ambientSound = s;
        }

        AmbientsoundManager.PlayAmbientSound();
      //  ambientSound.source.Play();
      //  ambientSound.source.PLA


    }
}
