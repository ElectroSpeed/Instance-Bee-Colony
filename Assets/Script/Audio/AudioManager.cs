using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private SoundLibrary _library;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic(SoundType.Child);
    }

    public void PlayMusic(SoundType type)
    {
        var sound = _library.GetSound(type, true);
        if (sound == null) return;
        _musicSource.clip = sound._clip;
        _musicSource.loop = sound._loop;
        _musicSource.Play();
    }

    public void PlaySFX(SoundType type)
    {
        var sound = _library.GetSound(type, false);
        if (sound == null) return;
        _sfxSource.PlayOneShot(sound._clip);
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }
    
    public void StopSFX()
    {
        _sfxSource.Stop();
    }
}