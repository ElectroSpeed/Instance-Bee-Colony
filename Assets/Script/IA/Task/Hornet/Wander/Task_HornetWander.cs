using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Task_HornetWander : AgentTaskBase
{
    private float _radius;
    private float _speed;
    private Vector3 _targetPosition;
    private bool _isFinished = false;

    private Transform _agentTransform;

    private float _scanInterval;
    private float _scanTimer;
    private float _scanRadius;

    private LayerMask _beeLayer;

    private PathFinding _pathFinder = new PathFinding();
    private List<Vector3> _currentPath;
    private int _pathIndex = 0;

    public Task_HornetWander(string taskName, Blackboard bb, float radius, float speed, float scanInterval, float scanRadius)
        : base(taskName, bb)
    {
        _radius = radius;
        _speed = speed;
        _scanInterval = scanInterval;
        _scanRadius = scanRadius;
        _beeLayer = LayerMask.GetMask("Bee");
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _isFinished = false;
        PickNewTarget();
    }

    private void PickNewTarget()
    {
        Vector2 rnd = Random.insideUnitCircle * _radius;
        _targetPosition = _agentTransform.position + new Vector3(rnd.x, 0, rnd.y);

        _currentPath = _pathFinder.FindPathPositions(_agentTransform.position, _targetPosition);
        _pathIndex = 0;

        if (_currentPath == null || _currentPath.Count == 0)
        {
            _isFinished = true;
            OnFinish();
        }
    }

    public override void OnUpdate()
    {
        FollowPath();

        _scanTimer += Time.deltaTime;
        if (_scanTimer >= _scanInterval)
        {
            _scanTimer = 0f;
            ScanForBees();
        }
    }

    private void FollowPath()
    {
        if (_isFinished || _currentPath == null || _pathIndex >= _currentPath.Count)
            return;

        Vector3 target = _currentPath[_pathIndex];
        _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, target, _speed * Time.deltaTime);

        Vector3 direction = (target - _agentTransform.position);
        if (direction.sqrMagnitude > 0.01f)
            _agentTransform.forward = direction.normalized;

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
        {
            _pathIndex++;
            if (_pathIndex >= _currentPath.Count)
            {
                _isFinished = true;
                OnFinish();
            }
        }
    }

    private void ScanForBees()
    {
        Collider[] hits = Physics.OverlapSphere(_agentTransform.position, _scanRadius, _beeLayer);
        

        foreach (var hit in hits)
        {
            _bb.ModifyValue("TargetBee", hit.transform);
            Debug.Log("Hornet found a bee to target.");
            return;
        }

        _bb.ModifyValue("TargetBee", null);
    }

    public override void OnFinish()
    {
        if (_isFinished)
        {
            _isFinished = false;
            PickNewTarget();
        }
    }

    public override void OnCancel() { }

    public override float GetUtility()
    {
        Transform targetBee = _bb.GetValue("TargetBee") as Transform;
        float cond_NoBee = targetBee == null ? 1f : 0.05f;
        return cond_NoBee;
    }


    public override int GetTaskPriority() => 0;

    public override bool IsTaskFinished() => _isFinished;
}
