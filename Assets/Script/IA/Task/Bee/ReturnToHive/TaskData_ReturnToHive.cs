using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/Agent/TaskData/Bee/ReturnToHive")]
public class TaskData_ReturnToHive : TaskDataBase<Task_ReturnToHive>
{
    public float _speed = 4f;

    protected override Task_ReturnToHive CreateTypedTask(string taskName, Blackboard bb)
    {
        return new Task_ReturnToHive(taskName, bb, _speed);
    }
}
