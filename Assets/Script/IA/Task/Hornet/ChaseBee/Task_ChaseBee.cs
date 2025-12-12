using UnityEngine;
using System.Collections.Generic;

public class Task_ChaseBee : AgentTaskBase
{
    private Transform _agentTransform;
    private Transform _targetBee;
    private float _speed;
    private PathFinding _pathFinder = new PathFinding();
    private List<Vector3> _currentPath;
    private int _pathIndex = 0;
    private bool _isFinished = false;

    public Task_ChaseBee(string taskName, Blackboard bb, float speed) : base(taskName, bb)
    {
        _speed = speed;
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _isFinished = false;
        UpdateTargetBee();
        ComputePath();
    }

    public override void OnUpdate()
    {
        UpdateTargetBee();

        if (_targetBee == null)
        {
            _isFinished = true;
            OnFinish();
            return;
        }

        FollowPath();
    }

    private void UpdateTargetBee()
    {
        _targetBee = _bb.GetValue("TargetBee") as Transform;
    }

    private void ComputePath()
    {
        if (_targetBee != null)
        {
            _currentPath = _pathFinder.FindPathPositions(_agentTransform.position, _targetBee.position);
            _pathIndex = 0;
        }
    }

    private void FollowPath()
    {
        if (_currentPath == null || _pathIndex >= _currentPath.Count)
        {
            ComputePath();
            return;
        }

        Vector3 target = _currentPath[_pathIndex];
        _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, target, _speed * Time.deltaTime);

        Vector3 direction = (target - _agentTransform.position);
        if (direction.sqrMagnitude > 0.01f)
            _agentTransform.forward = direction.normalized;

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
            _pathIndex++;
    }

    public override void OnFinish()
    {
        _isFinished = false;
        _currentPath = null;
        _pathIndex = 0;
    }

    public override void OnCancel()
    {
        _isFinished = true;
    }

    public override float GetUtility()
    {
        if (_targetBee != null)
        {
            float distance = Vector3.Distance(_agentTransform.position, _targetBee.position);
            return Mathf.Clamp(20f - distance, 0f, 20f);
        }
        return 0f;
    }

    public override int GetTaskPriority() => 1;

    public override bool IsTaskFinished() => _isFinished;
}
