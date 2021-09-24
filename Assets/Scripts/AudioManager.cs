using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static int currentTheme;
    public static bool currentThemePaused = true;
   
    public float setVolumeTo;

    private bool isFadingOut = false;
    private int newTheme;

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
        EventsManager.instance.AudioManagerPlay += OnAudioManagerPlay;
        EventsManager.instance.AudioManagerPause += OnAudioManagerPause;
        EventsManager.instance.AudioManagerVolumeDown += OnAudioManagerVolumeDown;
        EventsManager.instance.AudioManagerVolumeUp += OnAudioManagerVolumeUp;

        currentTheme = UnityEngine.Random.Range(0, sounds[0].clipList.Length);

        //StartCoroutine(FadeIn("Themes", currentTheme, 3));

    }

    private void Update()
    {

        if (!currentThemePaused)
        {
            Sound s = Array.Find(sounds, sound => sound.clipList[currentTheme]);

            if (s.source.time / s.source.clip.length > 0.90 && !isFadingOut)
            {
                StartCoroutine(FadeOut("Themes", currentTheme, 3));
            }

            if (!s.source.isPlaying && !currentThemePaused && !isFadingOut)
            {
                newTheme = UnityEngine.Random.Range(0, sounds[0].clipList.Length);

                while (newTheme == currentTheme)
                {
                    newTheme = UnityEngine.Random.Range(0, sounds[0].clipList.Length);
                }

                currentTheme = newTheme;
                ChangeToClip("Themes", currentTheme);
                   
            }
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
        StartCoroutine(FadeIn("Themes", clip, 3));
        //s.source.Play();
    }

    public void Play(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.clipList[clip]);
        s.source.clip = s.clipList[clip];
        currentThemePaused = false;

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
        currentThemePaused = true;

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }

        s.source.Pause();
        
    }

    public void VolumeDown(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }

        s.source.volume = setVolumeTo;

    }

    public void VolumeUp(string name, int clip)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.source.clip == sound.clipList[clip]);

        if (s == null)
        {
            Debug.LogWarning("Sound:" + name + " not found");
            return;
        }

        s.source.volume = s.volume;

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
        isFadingOut = true;
        float startVolume = s.source.volume;

        while (s.source.volume > 0)
        {
            s.source.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        s.source.Stop();
        s.source.volume = startVolume;
        isFadingOut = false;
    }

    public IEnumerator FadeIn(string name, int clip, float FadeTime)
    {
        currentThemePaused = false;
        Sound s = Array.Find(sounds, sound => sound.name == name && sound.clipList[clip]);

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

    void OnAudioManagerPlay()
    {
        StartCoroutine(FadeIn("Themes", currentTheme, 3));
    }

    void OnAudioManagerPause()
    {
        Pause("Themes", currentTheme);
    }

    void OnAudioManagerVolumeDown()
    {
        VolumeDown("Themes", currentTheme);
    }

    void OnAudioManagerVolumeUp()
    {
        VolumeUp("Themes", currentTheme);
    }

}