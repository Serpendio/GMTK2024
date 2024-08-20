using UnityEngine;
using UnityEngine.Audio;

public class AudioSingle : _Singleton<AudioSingle>
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource SFXPrefab;
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
    [field: SerializeField] public AudioClip slimeHit { get; private set; }
    [field: SerializeField] public AudioClip primeSlimeHit { get; private set; }
    [field: SerializeField] public AudioClip slimeSquash { get; private set; }
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
    /// <summary>
    /// play on the audiomanager
    /// </summary>
    public void PlaySFX(AudioClip clip, float volume = 1)
    {
        SFXSource.clip = clip;
        SFXSource.volume = volume;
        SFXSource.Play();
    }
    /// <summary>
    /// spawn a temporary SFX object
    /// </summary>
    public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1)
    {
        AudioSource audioSource = Instantiate(SFXPrefab, position, Quaternion.identity);
        audioSource.clip= clip;
        audioSource.volume= volume;
        audioSource.Play();

        Destroy(audioSource, clip.length);
    }
}
