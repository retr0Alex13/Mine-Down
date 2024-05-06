using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public Sound[] sounds;
    public AudioMixerGroup audioMixer;
    private static Dictionary<string, float> soundTimerDictionary;

    public static SoundManager instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        soundTimerDictionary = new Dictionary<string, float>();

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            foreach (AudioClip clip in sound.clips)
            {
                sound.source.clip = clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.isLoop;
                sound.source.outputAudioMixerGroup = audioMixer;
            }

            if (sound.hasCooldown)
            {
                Debug.Log(sound.name);
                soundTimerDictionary[sound.name] = 0f;
            }
        }
    }

    private void Start()
    {
        // Add this part after having a theme song
        // Play('Theme');
    }
    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }
        if (sound.clips.Length > 0)
        {
            sound.source.clip = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
        }
        else
        {
            sound.source.clip = sound.clips[0];
        }
    
        int clipIndex = Array.IndexOf(sound.clips, sound.source.clip);

        if (!CanPlaySound(sound, clipIndex)) return;
        sound.source.Play();
    }
    public void Play(string name, bool randomPitch)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }
        if (sound.clips.Length > 0)
        {
            sound.source.clip = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
        }
        else
        {
            sound.source.clip = sound.clips[0];
        }
        if (randomPitch)
        {
            sound.source.pitch = UnityEngine.Random.Range(1f, 1.5f);
        }
        int clipIndex = Array.IndexOf(sound.clips, sound.source.clip);

        if (!CanPlaySound(sound, clipIndex)) return;
        sound.source.Play();
    }

    public void Play(string name, Transform transform)
    {
        
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }
        if (sound.clips.Length > 0)
        {
            sound.source.clip = sound.clips[UnityEngine.Random.Range(0, sound.clips.Length)];
        }
        else
        {
            sound.source.clip = sound.clips[0];
        }

        int clipIndex = Array.IndexOf(sound.clips, sound.source.clip);

        if (!CanPlaySound(sound, clipIndex)) return;

        GameObject soundGameObject = new GameObject("Sound");

        soundGameObject.transform.parent = transform;
        soundGameObject.transform.localPosition = Vector3.zero;

        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        audioSource.volume = sound.volume;
        audioSource.pitch = sound.pitch;
        audioSource.loop = sound.isLoop;
        audioSource.clip = sound.clips[clipIndex];
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 10f;
        audioSource.outputAudioMixerGroup = audioMixer;
        //audioSource.minDistance = 0f;

        audioSource.Play();

        if (sound.isLoop)
        {
            return;
        }
        Destroy(soundGameObject, audioSource.clip.length);
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound " + name + " Not Found!");
            return;
        }

        sound.source.Stop();
    }

    private static bool CanPlaySound(Sound sound, int clipIndex)
    {

        if (soundTimerDictionary.ContainsKey(sound.name))
        {
            float lastTimePlayed = soundTimerDictionary[sound.name];

            if (lastTimePlayed + sound.clips[clipIndex].length < Time.time)
            {
                soundTimerDictionary[sound.name] = Time.time;
                return true;
            }

            return false;
        }

        return true;
    }
}