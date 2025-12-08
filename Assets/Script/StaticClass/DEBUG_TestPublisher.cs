using UnityEditor;
using UnityEngine;

public class DEBUG_TestPublisher : MonoBehaviour
{

    public float callback;

    [ContextMenu("TEST_EventPublisher")]
    public void TEST_EventPublisher()
    {
        EventBus.Publish<float>(EventType.NULL, callback);
    }
}

[CustomEditor(typeof(DEBUG_TestPublisher))]
public class DEBUG_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DEBUG_TestPublisher script = (DEBUG_TestPublisher)target;

        if (GUILayout.Button("Publish callBack"))
        {
            script.TEST_EventPublisher();
        }
    }
}