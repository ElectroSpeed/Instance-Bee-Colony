using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private AudioEventChannel _audioEventChannel;

    private const string _masterKey = "MasterVolume";
    private const string _musicKey = "MusicVolume";
    private const string _sfxKey = "SFXVolume";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        _audioEventChannel.OnMasterVolumeChanged += SetMasterVolume;
        _audioEventChannel.OnMusicVolumeChanged += SetMusicVolume;
        _audioEventChannel.OnSFXVolumeChanged += SetSFXVolume;
    }

    private void OnDisable()
    {
        _audioEventChannel.OnMasterVolumeChanged -= SetMasterVolume;
        _audioEventChannel.OnMusicVolumeChanged -= SetMusicVolume;
        _audioEventChannel.OnSFXVolumeChanged -= SetSFXVolume;
    }

    private void Start()
    {
        LoadVolumes();
    }

    private void LoadVolumes()
    {
        SetMasterVolume(PlayerPrefs.GetFloat(_masterKey, 1f));
        SetMusicVolume(PlayerPrefs.GetFloat(_musicKey, 1f));
        SetSFXVolume(PlayerPrefs.GetFloat(_sfxKey, 1f));
    }

    public void SetMasterVolume(float value)
    {
        _mixer.SetFloat("Master", value);
        PlayerPrefs.SetFloat(_masterKey, value);
    }

    public void SetMusicVolume(float value)
    {
        _mixer.SetFloat("Music", value);
        PlayerPrefs.SetFloat(_musicKey, value);
    }

    public void SetSFXVolume(float value)
    {
        _mixer.SetFloat("SFX", value);
        PlayerPrefs.SetFloat(_sfxKey, value);
    }
}