using System.Threading.Tasks;
using UnityEngine;

public class Task_AttackHornet : AgentTaskBase
{
    private Transform _agentTransform;
    private Transform _targetHornet;

    private float _attackRange;
    private float _attackCooldown;
    private float _attackDamage;

    private float _cooldownTimer;
    private bool _isFinished;

    public Task_AttackHornet(string taskName, Blackboard bb, Agent agent, float attackRange, float attackCooldown, float attackDamage) : base(taskName, bb, agent)
    {
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
        _attackRange = attackRange;
        _attackCooldown = attackCooldown;
        _attackDamage = attackDamage;
    }

    public override async Task OnStart()
    {
        _isFinished = false;
        _cooldownTimer = 0f;
        UpdateTarget();

        await base.OnStart();
    }

    public override void OnUpdate()
    {
        UpdateTarget();

        if (_targetHornet == null)
        {
            _bb.ModifyValue("IsUnderAttack", false);
            _isFinished = true;
            return;
        }

        float distance = Vector3.Distance(
            _agentTransform.position,
            _targetHornet.position
        );

        if (distance > _attackRange)
        {
            _isFinished = true;
            return;
        }

        Vector3 dir = _targetHornet.position - _agentTransform.position;
        if (dir.sqrMagnitude > 0.01f)
            _agentTransform.forward = dir.normalized;

        _cooldownTimer += Time.deltaTime;
        if (_cooldownTimer >= _attackCooldown)
        {
            _cooldownTimer = 0f;
            DealDamage();
        }
    }

    private void UpdateTarget()
    {
        _targetHornet = _bb.GetValue("TargetHornet") as Transform;
    }

    private void DealDamage()
    {
        if (_targetHornet == null) return;

        HornetAgent hornet = _targetHornet.GetComponent<HornetAgent>();
        if (hornet == null) return;

        Blackboard hornetBB = hornet.GetBlackboard();
        if (hornetBB == null) return;

        float currentHornetHealth = (float)hornetBB.GetValue("HealthHornet");
        currentHornetHealth -= _attackDamage;

        hornetBB.ModifyValue("HealthHornet", currentHornetHealth);

        Debug.Log("Bee attacks hornet, new health: " + currentHornetHealth);

        if (currentHornetHealth <= 0f)
        {
            _bb.ModifyValue("TargetHornet", null);
            _bb.ModifyValue("IsUnderAttack", false);
            Object.Destroy(hornet.gameObject);
        }
    }

    public override float GetUtility()
    {
        bool underAttack = (bool)(_bb.GetValue("IsUnderAttack") ?? false);
        return underAttack ? 2f : 0f;
    }

    public override int GetTaskPriority() => 3;

    public override bool IsTaskFinished() => _isFinished;

    public override async Task OnFinish()
    {
        await base.OnFinish();
        
    }

    public override void OnCancel()
    {
        _isFinished = true;
    }
}
