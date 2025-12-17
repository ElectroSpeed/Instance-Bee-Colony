using UnityEngine;

[CreateAssetMenu(menuName = ("GodGameSO/Agent/TaskData/Bee/Wander"))]
public class TaskData_Wander : TaskDataBase<Task_Wander>
{
    public float _radius;
    public float _scanCooldown = 0.5f;
    public float _scanTimer = 0f;
    public float _flowerDetectionRadius = 10f;

    protected override Task_Wander CreateTypedTask(string taskName, Blackboard bb, Agent agent)
    {
        return new Task_Wander(taskName, bb, agent, _radius, _scanCooldown, _scanTimer, _flowerDetectionRadius);
    }

}
    