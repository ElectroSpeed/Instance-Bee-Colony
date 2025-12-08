using UnityEngine;

public class DEBUG_EventTEST : MonoBehaviour
{
    public void OnEnable()
    {
        EventBus.Subscribe<float>(EventType.NULL, TEST);
    }
    public void OnDisable()
    {
        EventBus.Unsubscribe<float>(EventType.NULL, TEST);
    }
    public void TEST(float _callback)
    {
        print("event callback : " + _callback);    
    }
}
