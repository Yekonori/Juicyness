using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    [SerializeField] private float timeForMusicToFade = 1.5f;
    [SerializeField] private float musicVolume = 0.8f;

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
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == "Music");
        foreach (AudioSource source in fx.source)
        {
            if (!source.isPlaying)
            {
                source.clip = fx.clip[UnityEngine.Random.Range(0, fx.clip.Length)];
                source.Play();
                return;
            }
        }

        GameManager.instance.onStateChange += () =>
        {
            if(GameManager.instance.state != State.INGAME)
            {
                Stop("Music");
            }
        };
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

    public void Play(string name, float pitch)
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
                    source.pitch = pitch;
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

    public void PlayMusic()
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == "Music");
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : Music not found /!\\");
            return;
        }
        fx.source[0].clip = fx.clip[UnityEngine.Random.Range(0, fx.clip.Length)];
        StopCoroutine("GraduallyChangeMusicVolume");
        StartCoroutine(GraduallyChangeMusicVolume(fx.source[0], musicVolume, 0));
    }

    public void ChangeMusicPitch(float pitchToAdd)
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == "Music");
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : Music not found /!\\");
            return;
        }
        fx.source[0].pitch += pitchToAdd;
    }

    public void StopMusic()
    {
        SoundEffect fx = Array.Find(soundEffects, sound => sound.clipName == "Music");
        if (fx == null)
        {
            Debug.Log("/!\\ Sound : Music not found /!\\");
            return;
        }
        StopCoroutine("GraduallyChangeMusicVolume");
        StartCoroutine(GraduallyChangeMusicVolume(fx.source[0], 0, musicVolume));
    }

    private IEnumerator GraduallyChangeMusicVolume(AudioSource musicSource, float goal, float beginning)
    {
        float elapsedTime = 0;

        while (elapsedTime < timeForMusicToFade)
        {
            musicSource.volume = Mathf.Lerp(beginning, goal, (elapsedTime / timeForMusicToFade));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        // Make sure we got there
        musicSource.volume = goal;
        yield return null;
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
