using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Audio
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch = 0.5f;
    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

