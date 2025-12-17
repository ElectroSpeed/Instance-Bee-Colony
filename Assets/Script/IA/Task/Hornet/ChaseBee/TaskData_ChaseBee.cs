using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/Agent/TaskData/Hornet/ChaseBee")]
public class TaskData_ChaseBee : TaskDataBase<Task_ChaseBee>
{
    [SerializeField] private float _speed = 4f;

    protected override Task_ChaseBee CreateTypedTask(string taskName, Blackboard bb, Agent agent)
    {
        return new Task_ChaseBee(taskName, bb, agent, _speed);
    }
}
