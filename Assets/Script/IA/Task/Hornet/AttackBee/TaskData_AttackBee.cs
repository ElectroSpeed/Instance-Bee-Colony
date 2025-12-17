using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/Agent/TaskData/Hornet/AttackBee")]
public class TaskData_AttackBee : TaskDataBase<Task_AttackBee>
{
    public float _attackRange = 1.2f;
    public float _attackCooldown = 0.8f;
    public float _attackDamage = 10f;

    protected override Task_AttackBee CreateTypedTask(string taskName, Blackboard bb, Agent agent)
    {
        return new Task_AttackBee(taskName, bb, agent, _attackRange, _attackCooldown, _attackDamage);
    }
}
