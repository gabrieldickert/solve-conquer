using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static int currentTheme;
    public static bool currentThemePaused;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = s.attachTo.AddComponent<AudioSource>();
            //per default immer erste clip aus der clipList
            s.source.clip = s.clipList[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }
    
    void Start()
    {
        // 0 ist default clip
        currentTheme = 0;
        Play("Theme", currentTheme);
        currentThemePaused = false;
    }

    public void ChangeToClip(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
        s.source.clip = s.clipList[clip];
        //s.source.Play();
    }

    public void Play (string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.clipList[clip]);
        s.source.clip = s.clipList[clip];
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }
            
        s.source.Play();
    }

    public void Pause(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }
        s.source.Pause();
    }
    public void Stop(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }
        s.source.Stop();
    }

    public bool Playing(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);
        if(s == null)
        {
            return false;
        } else
        {
            return true;
        }
        
    }

    public IEnumerator FadeOut(string name, int clip, float FadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.source.Pause();
        s.source.volume = startVolume;
    }

    public IEnumerator FadeIn(string name, int clip, float FadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);
        s.source.clip = s.clipList[clip];
        float startVolume = s.source.volume;

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
