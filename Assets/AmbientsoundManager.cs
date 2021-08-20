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
    public float pitchMin;
    public float pitchMax;
    public static bool isActive = false;

    // Start is called before the first frame update
    private void Start()
    {
        AmbientsoundManager.instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            if (ambientSound.source.isPlaying)
            {
                Debug.Log("abspielen");
            }
            else
            {
                Debug.Log("vorbei");
                AmbientsoundManager.SelectedNewAmbientClip();
                ambientSound.source.Play();
            }
        }
    }

    public static void SelectedNewAmbientClip()
    {
        int randomIndex = Random.Range(0, ambientSound.clipList.Length);

        Debug.Log("RANDOM" + randomIndex);

        ambientSound.source.clip = ambientSound.clipList[randomIndex];
        ambientSound.source.volume = Random.Range(instance.randomVolMin, instance.randomVolMax);
        ambientSound.source.loop = false;
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

    public IEnumerator FadeOut(string name, int clip, float FadeTime)
    {
        Sound s = AmbientsoundManager.ambientSound;
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
        Sound s = AmbientsoundManager.ambientSound;
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