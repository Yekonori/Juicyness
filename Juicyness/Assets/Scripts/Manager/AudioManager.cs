﻿using System;
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
            fx.source = gameObject.AddComponent<AudioSource>();
            fx.source.volume = fx.volume;
            fx.source.priority = fx.priority;
            fx.source.loop = fx.loop;
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
            fx.source.clip = fx.clip[UnityEngine.Random.Range(0, fx.clip.Length)];
            fx.source.Play();
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
        fx.source.Stop();
    }
    
    public float GetClipLength(string name)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == name);
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : " + name + "not found /!\\");
            return 0;
        }
        return fx.source.clip.length;
    }
}
