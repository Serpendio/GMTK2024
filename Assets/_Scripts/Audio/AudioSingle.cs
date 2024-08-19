using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSingle : _Singleton<AudioSingle>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SVXSource;
    [SerializeField] [Range(0, 1)]
    private float _masterVolume = 1;
    public float MasterVolume
    {
        get => _masterVolume;
        set
        {
            _masterVolume = value;
            SetMasterVolume(_masterVolume);
        }
    }
    [SerializeField] [Range(0, 1)]
    private float _musicVolume = 1;
    public float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            SetMusicVolume(_musicVolume);
        }
    }

    [SerializeField] [Range(0, 1)]
    private float _sfxVolume = 1;
    public float SFXVolume
    {
        get => _sfxVolume;
        set
        {
            _sfxVolume = value;
            SetSFXVolume(_sfxVolume);
        }
    }
    private void Start()
    {
        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetSFXVolume(SFXVolume);

        musicSource.Play();
    }
    protected override void OnValidate()
    {
        base.OnValidate();

        SetMasterVolume(MasterVolume);
        SetMusicVolume(MusicVolume);
        SetSFXVolume(SFXVolume);
    }
    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master_Volume", value == 0 ? 0.0001f : Mathf.Log10(value) * 20);
    }
    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("Music_Volume", value == 0 ? 0.0001f : Mathf.Log10(value) * 20);
    }
    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX_Volume", value == 0 ? 0.0001f : Mathf.Log10(value) * 20);
    }

    public void PlaySFX(AudioClip clip, Vector3 position, float volume)
    {
        AudioSource audioSource = Instantiate(SVXSource, position, Quaternion.identity);
        audioSource.clip= clip;
        audioSource.volume= volume;
        audioSource.Play();

        Destroy(audioSource, clip.length);
    }
}
