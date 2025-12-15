using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : UIBase
{

    [SerializeField] private string nextScene="MVP Scene";
    
    public void Play()
    {
        SceneManager.LoadScene(nextScene);
    }


    
    public void Quit()
    {
        Application.Quit();
    }
    
    
}
