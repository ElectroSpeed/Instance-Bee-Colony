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

        if (Vector3.Distance(_agentTransform.position, targetPos) < 0.5f)
        {
            _isFinished = true;

        }
    }

    public override bool IsTaskFinished() => _isFinished;

    public override float GetUtility()
    {
        return _bb.GetValue("TargetFlower") != null ? 10f : 0f;
    }

    public override int GetTaskPriority() => 1;

    public override void OnFinish() { }
    public override void OnCancel() { }
}
