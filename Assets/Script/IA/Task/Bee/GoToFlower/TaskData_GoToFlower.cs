using UnityEngine;

[CreateAssetMenu(menuName = ("GodGameSO/Agent/TaskData/Bee/GoToFlower"))]
public class TaskData_GoToFlower : TaskDataBase<Task_GoToFlower>
{
    public float _speed;

    protected override Task_GoToFlower CreateTypedTask(string taskName, Blackboard bb)
    {
        return new Task_GoToFlower(taskName, bb, _speed);
    }

}
    