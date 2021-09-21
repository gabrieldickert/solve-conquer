using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static int currentTheme;
    public static bool currentThemePaused;

    private void Awake()
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

    private void Start()
    {
        // 0 ist default clip
        currentTheme = 0;
        Play("Themes", currentTheme);
    }

    private void Update()
    {
        
        if(currentTheme < sounds[0].clipList.Length)
        {
            Sound s = Array.Find(sounds, sound => sound.clipList[currentTheme]);
           
            if (!s.source.isPlaying)
            {
                currentTheme++;
                if (currentTheme < sounds[0].clipList.Length)
                {
                    ChangeToClip("Themes", currentTheme);
                }
            }
        } else
        {
            currentTheme = 0;
            ChangeToClip("Themes", currentTheme);
        }

    }

    public void ChangeToClip(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.clipList[clip]);

        if(s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }

        s.source.Stop();
        s.source.clip = s.clipList[clip];
        s.source.Play();
    }

    public void Play(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.clipList[clip]);
        s.source.clip = s.clipList[clip];

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }

        if (name.Equals("GlobalAmbient"))
        {
            AmbientsoundManager.StartPlayingAmbient(s);
        }
        else
        {
            s.source.Play();
        }
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
        if (s == null)
        {
            return false;
        }
        else
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