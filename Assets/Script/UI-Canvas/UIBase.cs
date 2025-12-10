using UnityEngine;

public class UIBase : MonoBehaviour
{   
    public void Activate(GameObject toActivate)
    {
        toActivate.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
