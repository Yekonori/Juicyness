using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    
    public SoundEffect[] soundEffects;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;

        foreach (SoundEffect fx in soundEffects)
        {
            if(fx.numberOfAudioSourceAvailable < 1)
            {
                fx.numberOfAudioSourceAvailable = 1;
            }
            fx.source = new AudioSource[fx.numberOfAudioSourceAvailable];
            for (int i = 0; i < fx.numberOfAudioSourceAvailable; i++)
            {
                fx.source[i] = gameObject.AddComponent<AudioSource>();
                fx.source[i].volume = fx.volume;
                fx.source[i].priority = fx.priority;
                fx.source[i].loop = fx.loop;
            }
        }
    }

    private void Start()
    {
    }

    public void Play(string name)
    {
        if (FeatureManager.instance.isSoundEffectOn)
        {
            SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
            if (fx == null)
            {
                Debug.Log("/!\\ Sound : " + name + "not found /!\\");
                return;
            }
            foreach (AudioSource source in fx.source)
            {
                if (!source.isPlaying)
                {
                    source.clip = fx.clip[UnityEngine.Random.Range(0, fx.clip.Length)];
                    source.Play();
                    return;
                }
            }
        }
    }

    public void Stop(string name)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : " + name + "not found /!\\");
            return;
        }
        foreach (AudioSource source in fx.source)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }
    
    public float GetClipLength(string name)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : " + name + "not found /!\\");
            return 0;
        }
        return fx.source[0].clip.length;
    }
}
