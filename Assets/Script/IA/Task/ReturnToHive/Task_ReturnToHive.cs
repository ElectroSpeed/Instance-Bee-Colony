using UnityEngine;

public class Task_ReturnToHive : AgentTaskBase
{
    private Transform _agentTransform;
    private bool _isFinished = false;
    private float _speed;

    private float _waitDuration = 1.5f;
    private float _timer = 0f;
    private bool _arrived = false;

    public Task_ReturnToHive(string name, Blackboard bb, float speed)
        : base(name, bb)
    {
        _speed = speed;
    }

    public override void OnStart()
    {
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");

        if (_bb.GetValue("Hive") == null)
        {
            _isFinished = true;
            return;
        }

        _isFinished = false;
        _timer = 0f;
    }

    public override void OnUpdate()
    {
        Beehive hive = (Beehive)_bb.GetValue("Hive");
        if (hive == null)
        {
            _isFinished = true;
            return;
        }

        Vector3 target = hive.transform.position;

        if (!_arrived)
        {
            _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, target, _speed * Time.deltaTime);

            if (Vector3.Distance(_agentTransform.position, target) < 0.5f)
            {
                _arrived = true;
                _timer = 0f;
            }
        }
        else
        {
            _timer += Time.deltaTime;

            if (_timer >= _waitDuration)
            {
                int pollen = (int)_bb.GetValue("CollectedPollen");
                if (pollen > 0)
                {
                    hive.AddPollen(pollen);
                    _bb.ModifyValue("CollectedPollen", 0);
                }
                _isFinished = true;
            }
        }
    }

    public override bool IsTaskFinished() => _isFinished;

    public override void OnFinish() { }
    public override void OnCancel() { }

    public override int GetTaskPriority() => 3;

    public override float GetUtility()
    {
        int pollen = (int)_bb.GetValue("CollectedPollen");
        if (pollen <= 0) return 0f;

        Hunger h = GetHunger();
        Tiredness t = GetTiredness();

        float cond_HasPollen = 1f;
        float cond_NotHungry = Normalize(h._currentHungerValue, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t._currentTirednessValue, t._minTirednessValue, t._maxTirednessValue);

        return Combine(cond_HasPollen, cond_NotHungry, cond_NotTired);
    }
}
