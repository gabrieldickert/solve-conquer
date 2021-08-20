using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static string currentTheme;
    public static bool currentThemePaused;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = s.attachTo.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
        }
    }
    
    void Start()
    {
        Play("Theme");
        currentTheme = "Theme";
        currentThemePaused = false;
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }
            
        s.source.Play();
    }

    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public bool Playing(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }

    public IEnumerator FadeOut(string name, float FadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.source.Pause();
        s.source.volume = startVolume;
    }

    public IEnumerator FadeIn(string name, float FadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
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
