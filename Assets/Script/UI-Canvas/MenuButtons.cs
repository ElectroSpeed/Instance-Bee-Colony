using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    [SerializeField] private string _sceneMainMenuName= "Main Menu Raphaël";
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
    }

    public void LaunchGame()
    {
        SceneManager.LoadScene(_scenePlayName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
