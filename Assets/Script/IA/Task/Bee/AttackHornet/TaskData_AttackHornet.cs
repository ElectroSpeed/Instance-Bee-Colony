using UnityEngine;

[CreateAssetMenu(menuName = "GodGameSO/Agent/TaskData/Bee/AttackHornet")]
public class TaskData_AttackHornet : TaskDataBase<Task_AttackHornet>
{
    public float _attackRange = 2f;
    public float _attackCooldown = 1f;
    public float _attackDamage = 10f;

    protected override Task_AttackHornet CreateTypedTask(string taskName, Blackboard bb)
    {
        return new Task_AttackHornet(taskName, bb, _attackRange, _attackCooldown, _attackDamage);
    }
}
