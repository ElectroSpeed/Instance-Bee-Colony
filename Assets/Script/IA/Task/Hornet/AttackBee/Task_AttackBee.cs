using UnityEngine;

public class Task_AttackBee : AgentTaskBase
{
    private Transform _agentTransform;
    private Transform _targetBee;

    private float _attackRange;
    private float _attackCooldown;
    private float _attackDamage;

    private float _cooldownAttackTimer;
    private bool _isFinished;

    public Task_AttackBee(string taskName, Blackboard bb, float attackRange, float attackCooldown, float attackDamage) : base(taskName, bb)
    {
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _attackDamage = attackDamage;
    }

    public override void OnStart()
    {
        _isFinished = false;
        _cooldownAttackTimer = 0f;
        UpdateTarget();
    }

    public override void OnUpdate()
    {
        UpdateTarget();

        if (_targetBee == null)
        {
            _isFinished = true;
            return;
        }

        float distance = Vector3.Distance(
            _agentTransform.position,
            _targetBee.position
        );

        if (distance > _attackRange)
        {
            _isFinished = true;
            return;
        }

        Vector3 dir = (_targetBee.position - _agentTransform.position);
        if (dir.sqrMagnitude > 0.01f)
            _agentTransform.forward = dir.normalized;

        _cooldownAttackTimer += Time.deltaTime;
        if (_cooldownAttackTimer >= _attackCooldown)
        {
            _cooldownAttackTimer = 0f;
            DealDamage();
        }
    }

    private void UpdateTarget()
    {
        _targetBee = _bb.GetValue("TargetBee") as Transform;
    }

    private void DealDamage()
    {
        if (_targetBee == null) return;

        BeeAgent beeAgent = _targetBee.GetComponentInParent<BeeAgent>();
        if (beeAgent == null) return;

        Blackboard beeBB = beeAgent.GetBlackboard();
        if (beeBB == null) return;

        if (beeBB.GetValue("Health") is Health health)
        {
            health.Current -= _attackDamage;

            Debug.Log("Hornet attacks bee, new health: " + health.Current);

            beeBB.ModifyValue("Health", health);
            beeBB.ModifyValue("IsUnderAttack", true);
            beeBB.ModifyValue("TargetHornet", _agentTransform);
            Debug.Log("HP of the bee: " + health.Current);
            if (health.Current <= 0f)
            {
                beeAgent.Die();
            }
        }
    }

    public override void OnFinish() { }

    public override void OnCancel()
    {
        _isFinished = true;
    }

    public override float GetUtility()
    {
        Transform targetBee = _bb.GetValue("TargetBee") as Transform;
        if (targetBee == null) return 0f;

        float distance = Vector3.Distance(_agentTransform.position, targetBee.position);

        return distance <= _attackRange ? 1f : 0f;
    }

    public override int GetTaskPriority() => 2;

    public override bool IsTaskFinished() => _isFinished;
}
