using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    public List<Sound> _musicSounds = new();
    public List<Sound> _sfxSounds = new();

    public Sound GetSound(SoundType type, bool isMusic)
    {
        string soundName = type.ToString();
        List<Sound> soundList = isMusic ? _musicSounds : _sfxSounds;
        foreach (Sound sound in soundList)
            if (sound._name == soundName)
                return sound;
        Debug.LogWarning($"Son {soundName} introuvable dans {(isMusic ? "Music" : "SFX")}.");
        return null;
    }
}
