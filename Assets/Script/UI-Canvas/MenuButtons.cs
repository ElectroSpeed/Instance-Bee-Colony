using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    [SerializeField] private string _sceneMainMenuName= "Main Menu";
    [SerializeField] private string _scenePlayName= "MVP Scene";
    
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_sceneMainMenuName);
        AudioManager.Instance.PlayMusic(SoundType.MusicMenu);
    }

    public void LaunchGame()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlayMusic(SoundType.MainMusic);
        SceneManager.LoadScene(_scenePlayName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
