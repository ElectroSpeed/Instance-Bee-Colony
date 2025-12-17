using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/Agent/TaskData/Hornet/Wander")]
public class TaskData_HornetWander : TaskDataBase<Task_HornetWander>
{
    public float _radius = 15f;
    public float _moveSpeed = 3f;
    public float _scanInterval;
    public float _scanRadius;


    protected override Task_HornetWander CreateTypedTask(string taskName, Blackboard bb, Agent agent)
    {
        return new Task_HornetWander(taskName, bb, agent, _radius, _moveSpeed, _scanInterval, _scanRadius);
    }
}
