using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    void Start()
    {                   //si existe el playerpref
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }
    void Update()
    {
        
    }
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        //para convertir el slider en logaritmico. Es decir, coger del 0,0001 pasando por los decimales hasta 1.
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        //guardar el volumen
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        //para convertir el slider en logaritmico. Es decir, coger del 0,0001 pasando por los decimales hasta 1.
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        //guardar el volumen
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    private void LoadVolume()
    {
        //guardar el progreso del slider y decirle q era su volumen.
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}
