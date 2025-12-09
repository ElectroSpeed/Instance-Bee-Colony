using UnityEngine;

[CreateAssetMenu(menuName = ("GodGameSO/Agent/TaskData/CollectFlower"))]
public class TaskData_CollectFlower : TaskDataBase<Task_CollectFlower>
{
    public int _pollentCollected;

    protected override Task_CollectFlower CreateTypedTask(string taskName, Blackboard bb)
    {
        return new Task_CollectFlower (taskName, bb, _pollentCollected);
    }

}
    