using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private SoundLibrary _soundLibrary;

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

    public void PlayMusic(SoundType type)
    {
        var sound = _soundLibrary.GetSound(type, true);
        if (sound == null) return;

        if (_musicSource.clip == sound._clip) return;

        _musicSource.clip = sound._clip;
        _musicSource.loop = sound._loop;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void PlaySFX(SoundType type)
    {
        var sound = _soundLibrary.GetSound(type, false);
        if (sound == null) return;

        _sfxSource.PlayOneShot(sound._clip);
    }

    public void PlaySFXInObject(SoundType type, AudioSource source)
    {
        var sound = _soundLibrary.GetSound(type, false);
        if (sound == null) return;
        
        source.PlayOneShot(sound._clip);
    }
}