using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Audio[] sounds;

    private void Awake()
    {
        foreach (Audio sound in sounds) 
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void Play(string name) 
    {
        Audio s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        else
            Debug.LogWarning("Sound " + name + " was not found.");
    }

    public void Stop(string name) 
    {
        Audio s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Stop();
        else
            Debug.LogWarning("Sound " + name + " was not found.");
    }
}
