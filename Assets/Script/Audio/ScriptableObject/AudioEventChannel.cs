using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Audio Event Channel")]
public class AudioEventChannel : ScriptableObject
{
    public Action<float> OnMasterVolumeChanged;
    public Action<float> OnMusicVolumeChanged;
    public Action<float> OnSFXVolumeChanged;
}