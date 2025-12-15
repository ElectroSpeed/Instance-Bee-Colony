using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/Agent/TaskData/Bee/Rest")]
public class TaskData_Rest : TaskDataBase<Task_Rest>
{
    public float _restDuration;

    protected override Task_Rest CreateTypedTask(string taskName, Blackboard bb)
    {
        return new Task_Rest(taskName, bb, _restDuration);
    }
}
