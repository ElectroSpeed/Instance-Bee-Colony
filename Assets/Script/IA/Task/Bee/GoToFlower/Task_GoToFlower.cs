using UnityEngine;
using System.Collections.Generic;

public class Task_GoToFlower : AgentTaskBase
{
    private Flower _targetFlower;
    private Transform _agentTransform;

    private float _speed = 4f;
    private bool _isFinished = false;

    private PathFinding _pathFinder = new PathFinding();
    private List<Vector3> _currentPath;
    private int _pathIndex = 0;

    public Task_GoToFlower(string name, Blackboard bb, float speed) : base(name, bb)
    {
        _speed = speed;
        _agentTransform = (Transform)bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _targetFlower = (Flower)_bb.GetValue("TargetFlower");
        _isFinished = false;

        if (_targetFlower == null)
        {
            _isFinished = true;
            return;
        }

        Vector3 targetPos = _targetFlower.transform.position;

        _currentPath = _pathFinder.FindPathPositions(_agentTransform.position, targetPos);
        _pathIndex = 0;

        if (_currentPath == null || _currentPath.Count == 0)
        {
            _isFinished = true;
        }
    }

    public override void OnUpdate()
    {
        if (_isFinished || _targetFlower == null)
        {
            _isFinished = true;
            return;
        }

        FollowPath();
    }

    private void FollowPath()
    {
        if (_currentPath == null || _pathIndex >= _currentPath.Count)
        {
            _isFinished = true;
            return;
        }

        Vector3 target = _currentPath[_pathIndex];

        _agentTransform.position = Vector3.MoveTowards(
            _agentTransform.position,
            target,
            _speed * Time.deltaTime
        );

        Vector3 dir = (target - _agentTransform.position);
        if (dir.sqrMagnitude > 0.01f)
            _agentTransform.forward = dir.normalized;

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
        {
            _pathIndex++;

            if (_pathIndex >= _currentPath.Count)
            {
                _isFinished = true;
            }
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
        float cond_NotHungry = Normalize(h.Current, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t.Current, t._minTirednessValue, t._maxTirednessValue);

        float cond_InventoryEmpty = ((int)_bb.GetValue("CollectedPollen") == 0) ? 1f : 0f;

        Transform agent = _agentTransform;
        float dist = Vector3.Distance(agent.position, flower.transform.position);

        float cond_NotCloseYet = dist > 0.5f ? 1f : 0f;

        return Combine(cond_HasFlower, cond_NotHungry, cond_NotTired, cond_InventoryEmpty, cond_NotCloseYet);
    }

    public override int GetTaskPriority() => 1;

    public override void OnFinish() { }
    public override void OnCancel() { }
}
