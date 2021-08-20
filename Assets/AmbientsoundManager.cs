using System.Collections;
using UnityEngine;

public class AmbientsoundManager : MonoBehaviour
{
    public static Sound ambientSound;
    public static AmbientsoundManager instance;
    public bool playAmbientSound;
    public float randomVolMin;
    public float randomVolMax;
    public bool allowToSkip;
    private bool skipTrack = false;
    public float pitchMin;
    public float pitchMax;
    public int skipWaitTime = 5;
    public static bool isActive = false;


    // Start is called before the first frame update
    private void Start()
    {
        AmbientsoundManager.instance = this;

        if(allowToSkip)
        {
            skipTrack = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {

      
        if (isActive)
        {
            if (ambientSound.source.isPlaying)
            {
            }
            else
            {
                AmbientsoundManager.SelectedNewAmbientClip();
            }
        }
    }

    public static void SelectedNewAmbientClip()
    {
        int randomIndex = Random.Range(0, ambientSound.clipList.Length+3);

        if(randomIndex > ambientSound.clipList.Length)
        {
            AmbientsoundManager.instance.skipTrack = true;
        }

        if(!AmbientsoundManager.instance.skipTrack)
        {
            instance.StartCoroutine(AmbientsoundManager.instance.FadeIn(randomIndex, 1f));

           
        }
        else
        {

            AmbientsoundManager.instance.StartCoroutine(AmbientsoundManager.instance.SkipWaiter());
        }
        
        Debug.Log("RANDOM" + randomIndex);
       /* ambientSound.source.clip = ambientSound.clipList[randomIndex];
        ambientSound.source.volume = Random.Range(instance.randomVolMin, instance.randomVolMax);
        ambientSound.source.Play();*/


    }
    IEnumerator SkipWaiter()
    { 
        yield return new WaitForSeconds(AmbientsoundManager.instance.skipWaitTime);

    }

    public static void StartPlayingAmbient(Sound s)
    {
        if (ambientSound == null)
        {
            ambientSound = s;
        }

        ambientSound.source.loop = false;
        isActive = true;

    }

    public IEnumerator FadeOut(int clip, float FadeTime)
    {
        Sound s = AmbientsoundManager.ambientSound;
        float startVolume = s.source.volume;
        s.source.loop = false;


        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.source.Pause();
        s.source.volume = startVolume;
    }

    public IEnumerator FadeIn( int clip, float FadeTime)
    {
        Sound s = AmbientsoundManager.ambientSound;
        s.source.clip = s.clipList[clip];
        float startVolume = s.source.volume;
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
}