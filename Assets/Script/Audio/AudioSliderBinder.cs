using UnityEngine;
using UnityEngine.UI;

public class AudioSliderBinder : MonoBehaviour
{
    [SerializeField] private AudioEventChannel _audioEventChannel;
    [SerializeField] private ChannelType _channelType;
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        if (!_slider)
            _slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        _slider.onValueChanged.AddListener(HandleValueChanged);
        LoadSavedValue();
    }

    private void OnDisable()
    {
        _slider.onValueChanged.RemoveListener(HandleValueChanged);
    }

    private void LoadSavedValue()
    {
        switch (_channelType)
        {
            case ChannelType.Master:
                _slider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
                break;

            case ChannelType.Music:
                _slider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
                break;

            case ChannelType.Sfx:
                _slider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
                break;
        }
    }

    private void HandleValueChanged(float value)
    {
        switch (_channelType)
        {
            case ChannelType.Master:
                _audioEventChannel.OnMasterVolumeChanged?.Invoke(value);
                break;

            case ChannelType.Music:
                _audioEventChannel.OnMusicVolumeChanged?.Invoke(value);
                break;

            case ChannelType.Sfx:
                _audioEventChannel.OnSFXVolumeChanged?.Invoke(value);
                break;
        }
    }
}