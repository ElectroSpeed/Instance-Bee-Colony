using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButton : UIBase
{

    public void Play()
    {
        SceneManager.LoadScene("Merge Scene");
    }


    
    public void Quit()
    {
        Application.Quit();
    }
    
    
}
