using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip ambient;
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void ReproducirSFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
