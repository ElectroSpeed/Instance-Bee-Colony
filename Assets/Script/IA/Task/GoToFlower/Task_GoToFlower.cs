using UnityEngine;

public class Task_GoToFlower : AgentTaskBase
{
    private Flower _targetFlower;
    private Transform _agentTransform;
    private float _speed = 4f;
    private bool _isFinished = false;


    public Task_GoToFlower(string name, Blackboard bb, float speed) : base(name, bb)
    {
        _speed = speed;
        _agentTransform = (Transform)bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _targetFlower = (Flower)_bb.GetValue("TargetFlower");
        _isFinished = false;
    }

    public override void OnUpdate()
    {
        if (_targetFlower == null)
        {
            _isFinished = true;
            return;
        }

        Vector3 targetPos = _targetFlower.transform.position;
        _agentTransform.position = Vector3.MoveTowards(
            _agentTransform.position,
            targetPos,
            _speed * Time.deltaTime
        );

        if (Vector3.Distance(_agentTransform.position, targetPos) == 0f)
        {
            _isFinished = true;

        }
    }

    public override bool IsTaskFinished() => _isFinished;

    public override float GetUtility()
    {
        Flower flower = (Flower)_bb.GetValue("TargetFlower");
        if (flower == null) return 0f;

        Hunger h = GetHunger();
        Tiredness t = GetTiredness();

        float cond_HasFlower = 1f;
        float cond_NotHungry = Normalize(h._currentHungerValue, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t._currentTirednessValue, t._minTirednessValue, t._maxTirednessValue);

        float cond_InventoryEmpty = ((int)_bb.GetValue("CollectedPollen") == 0) ? 1f : 0f;

        Transform agent = (Transform)_bb.GetValue("AgentTransform");
        float dist = Vector3.Distance(agent.position, flower.transform.position);

        float cond_NotCloseYet = dist > 0.5f ? 1f : 0f;

        return Combine(cond_HasFlower, cond_NotHungry, cond_NotTired, cond_InventoryEmpty, cond_NotCloseYet);
    }

    public override int GetTaskPriority() => 1;

    public override void OnFinish() { }
    public override void OnCancel() { }
}
