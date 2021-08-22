using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientEntityScript : MonoBehaviour
{
    public Sound songObj;
    public float SecondsBetweenNewAmbientsound;
    public float randomVolMin;
    public float randomVolMax;
    public bool allowToSkip;
    private bool skipTrack = false;
    public float pitchMin;
    public float pitchMax;
    public float panMin;
    public float panMax;
    public float fadeoutPercentage;


    void Start()
    {
        //Setting DefaultSettings
        songObj.source.clip = songObj.clipList[Random.Range(0, songObj.clipList.Length)];
        songObj.source.volume = songObj.volume;
        songObj.source.pitch = songObj.pitch;
        songObj.source.loop = songObj.loop;
        songObj.source.spatialBlend = songObj.spatialBlend;
        //Attaching Song to the Object containg the Entity script
        songObj.attachTo = this.gameObject;

        //Checking a Track can be skipped
        if (allowToSkip)
        {
            skipTrack = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

            if (songObj.source.isPlaying)
            {

                if ((songObj.source.time / songObj.source.clip.length * 100) > fadeoutPercentage)
                {
                   StartCoroutine(FadeOut(1f));
                }
            }
            else
            {
                StartCoroutine(StartNewSong());

            }
        
    }

    public IEnumerator FadeOut(float FadeTime)
    {
        float startVolume = songObj.source.volume;
        songObj.source.loop = false;


        while (songObj.source.volume > 0)
        {
            songObj.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        songObj.source.Pause();
        songObj.source.volume = startVolume;
    }
}
