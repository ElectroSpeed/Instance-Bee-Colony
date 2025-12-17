using UnityEngine;
using System.Collections.Generic;

public class Task_ChaseBee : AgentTaskBase
{
    private Transform _agentTransform;
    private Transform _targetBee;
    private float _speed;

    private List<Vector3> _currentPath;
    private int _pathIndex;
    private bool _isFinished;

    // Path state
    private bool _isWaitingForPath;
    private int _pathRequestId;

    // Chase variables
    private Vector3 _lastTargetPos;
    private Vector3 _lastAgentPos;
    private float _repathInterval = 0.3f;
    private float _repathTimer;
    private float _repathDistance = 1.0f;
    private float _minMoveBeforeRepath = 0.5f;

    public Task_ChaseBee(string taskName, Blackboard bb, Agent agent, float speed)
        : base(taskName, bb, agent)
    {
        _speed = speed;
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _isFinished = false;
        _currentPath = null;
        _pathIndex = 0;
        _repathTimer = 0f;
        _isWaitingForPath = false;

        UpdateTargetBee();

        if (_targetBee == null)
        {
            _isFinished = true;
            return;
        }

        _lastAgentPos = _agentTransform.position;
        _lastTargetPos = _targetBee.position;

        RequestPath();
    }

    public override void OnUpdate()
    {
        UpdateTargetBee();

        if (_targetBee == null)
        {
            _isFinished = true;
            return;
        }

        _repathTimer += Time.deltaTime;

        if (!_isWaitingForPath && CanRepath())
        {
            RequestPath();
        }

        if (_currentPath == null)
            return;

        FollowPath();
    }

    private void RequestPath()
    {
        _isWaitingForPath = true;
        int requestId = ++_pathRequestId;

        _agent.StartGetPathPoint(
            _agentTransform.position,
            _targetBee.position,
            path =>
            {
                if (requestId != _pathRequestId)
                    return;

                OnPathReady(path);
            }
        );

        _repathTimer = 0f;
        _lastAgentPos = _agentTransform.position;
        _lastTargetPos = _targetBee.position;
    }

    private void OnPathReady(List<Vector3> path)
    {
        _isWaitingForPath = false;

        if (path == null || path.Count == 0)
            return;

        _currentPath = path;
        _pathIndex = FindClosestPathIndex(_currentPath, _agentTransform.position);

        if (_pathIndex < _currentPath.Count - 1)
            _pathIndex++;
    }

    private bool CanRepath()
    {
        float targetMoved = Vector3.Distance(_lastTargetPos, _targetBee.position);
        float agentMoved = Vector3.Distance(_lastAgentPos, _agentTransform.position);

        return _repathTimer >= _repathInterval
            && targetMoved >= _repathDistance
            && agentMoved >= _minMoveBeforeRepath;
    }

    private void FollowPath()
    {
        if (_pathIndex >= _currentPath.Count)
            return;

        Vector3 target = _currentPath[_pathIndex];

        _agentTransform.position = Vector3.MoveTowards(
            _agentTransform.position,
            target,
            _speed * Time.deltaTime
        );

        Vector3 dir = target - _agentTransform.position;
        if (dir.sqrMagnitude > 0.01f)
            _agentTransform.forward = dir.normalized;

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
            _pathIndex++;
    }

    private void UpdateTargetBee()
    {
        _targetBee = _bb.GetValue("TargetBee") as Transform;
    }

    private int FindClosestPathIndex(List<Vector3> path, Vector3 agentPos)
    {
        int closestIndex = 0;
        float closestDist = float.MaxValue;

        for (int i = 0; i < path.Count; i++)
        {
            float dist = (path[i] - agentPos).sqrMagnitude;
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    public override void OnFinish()
    {
        _currentPath = null;
        _pathIndex = 0;
        _isWaitingForPath = false;
        _pathRequestId++;
        _isFinished = false;
    }

    public override void OnCancel()
    {
        _pathRequestId++;
        _currentPath = null;
        _isFinished = true;
    }

    public override float GetUtility()
    {
        Transform target = _bb.GetValue("TargetBee") as Transform;
        if (target == null) return 0f;

        float maxDistance = 30f;
        float d = Vector3.Distance(_agentTransform.position, target.position);
        return Mathf.Clamp01(1f - d / maxDistance);
    }

    public override int GetTaskPriority() => 1;
    public override bool IsTaskFinished() => _isFinished;
}
