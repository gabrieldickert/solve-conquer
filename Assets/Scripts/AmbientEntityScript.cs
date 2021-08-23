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
    public float pitchMin;
    public float pitchMax;
    public float panMin;
    public float panMax;
    public float fadeoutPercentage;
    public float fadeDuration = 1f;

    private float startTime = 0f;
    private bool isFadingOut = false;

    void Start()
    {

        this.gameObject.AddComponent<AudioSource>();
        songObj.attachTo = this.gameObject;
        songObj.source = this.gameObject.GetComponent<AudioSource>();

        //Setting DefaultSettings
        songObj.source.clip = songObj.clipList[0];
        songObj.source.volume = songObj.volume;
        songObj.source.pitch = songObj.pitch;
        songObj.source.loop = songObj.loop;
        songObj.source.spatialBlend = songObj.spatialBlend;
        this.startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (songObj.source.isPlaying)
        {
            if ((songObj.source.time / songObj.source.clip.length) * 100 > fadeoutPercentage && !this.isFadingOut)
            {
                StartCoroutine(FadeOut(this.fadeDuration * songObj.source.clip.length));
            }
        }
        else if(Time.time > this.startTime + this.SecondsBetweenNewAmbientsound)
        {
            StartNewSong();
        }

        
    }

    void StartNewSong()
    {
        this.startTime = Time.time;
        if (allowToSkip)
        {
            bool skipSong = System.Convert.ToBoolean(Random.Range(0, 1));
            if (!skipSong)
            {
                SelectedNewAmbientClip();
            }
        } else
        {
            SelectedNewAmbientClip();
        }
    }
    public void SelectedNewAmbientClip()
    {
        int randomIndex = Random.Range(0, songObj.clipList.Length);
        StartCoroutine(FadeIn(randomIndex, this.fadeDuration * songObj.source.clip.length));
    }


    

    public IEnumerator FadeIn(int clip, float FadeTime)
    {
        Sound s = songObj;
        
        s.source.clip = s.clipList[clip];
        s.source.panStereo = Random.Range(panMin, panMax);
        s.source.pitch = Random.Range(pitchMin,pitchMax);
        s.source.spatialBlend = s.spatialBlend;
        float startVolume = Random.Range(randomVolMin,randomVolMax);
        s.source.loop = false;
        s.source.volume = 0;

        
        s.source.Play();
        
        while (s.source.volume < startVolume)
        {
            s.source.volume += startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }
        
        s.source.volume = startVolume;
    }
    public IEnumerator FadeOut(float FadeTime)
    {
        this.isFadingOut = true;
        float startVolume = songObj.source.volume;
        songObj.source.loop = false;
        
        while (songObj.source.volume > 0)
        {
            songObj.source.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        this.isFadingOut = false;
        songObj.source.Stop();
        songObj.source.volume = startVolume;
    }
}
