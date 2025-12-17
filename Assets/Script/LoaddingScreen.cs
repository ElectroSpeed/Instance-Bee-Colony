using UnityEngine;

public class LoaddingScreen : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject blackScreen;

    private float loaddingTime = 1f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= loaddingTime)
        {
            pauseScreen.SetActive(false);
            blackScreen.SetActive(false);
        }
    }
}
