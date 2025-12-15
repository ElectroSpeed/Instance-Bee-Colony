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

    //Chase variable 
    private Vector3 _lastTargetPos;
    private Vector3 _lastAgentPos;
    private float _repathInterval = 0.3f;
    private float _repathTimer = 0f;
    private float _repathDistance = 1.0f;
    private float _minMoveBeforeRepath = 0.5f;


    public Task_ChaseBee(string taskName, Blackboard bb, float speed) : base(taskName, bb)
    {
        _speed = speed;
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _isFinished = false;
        UpdateTargetBee();

        _lastAgentPos = _agentTransform.position;

        if (_targetBee != null)
        {
            _lastTargetPos = _targetBee.position;
            ComputePath();
        }
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

        _repathTimer += Time.deltaTime;

        float targetMovedDistance = Vector3.Distance(_lastTargetPos, _targetBee.position);
        float agentMovedDistance = Vector3.Distance(_lastAgentPos, _agentTransform.position);

        bool canRepath = _repathTimer >= _repathInterval && targetMovedDistance >= _repathDistance && agentMovedDistance >= _minMoveBeforeRepath;

        if (canRepath)
        {
            _repathTimer = 0f;
            _lastTargetPos = _targetBee.position;
            _lastAgentPos = _agentTransform.position;
            ComputePath();

            if (_currentPath.Count > 1 && Vector3.Distance(_agentTransform.position, _currentPath[0]) < 0.2f)
            {
                _pathIndex = 1;
            }
        }

        FollowPath();
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
            float dist = Vector3.SqrMagnitude(path[i] - agentPos);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }

    private void ComputePath()
    {
        if (_targetBee == null) return;

        _currentPath = _pathFinder.FindPathPositions(
            _agentTransform.position,
            _targetBee.position
        );

        if (_currentPath == null || _currentPath.Count == 0)
            return;

        _pathIndex = FindClosestPathIndex(_currentPath, _agentTransform.position);

        if (_pathIndex < _currentPath.Count - 1)
            _pathIndex++;
    }

    private void FollowPath()
    {
        if (_currentPath == null || _currentPath.Count == 0)
            return;

        if (_pathIndex >= _currentPath.Count)
            return;

        Vector3 target = _currentPath[_pathIndex];

        _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, target, _speed * Time.deltaTime);

        Vector3 direction = (target - _agentTransform.position);
        if (direction.sqrMagnitude > 0.01f)
        {
            _agentTransform.forward = direction.normalized;
        }

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
        {
            _pathIndex++;
        }
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
        Transform targetBee = _bb.GetValue("TargetBee") as Transform;
        if (targetBee == null) return 0f;

        float maxConsiderDistance = 30f;    
        float distance = Vector3.Distance(_agentTransform.position, targetBee.position);
        float u = Mathf.Clamp01(1f - (distance / maxConsiderDistance));
        return u;
    }

    public override int GetTaskPriority() => 1;

    public override bool IsTaskFinished() => _isFinished;
}
