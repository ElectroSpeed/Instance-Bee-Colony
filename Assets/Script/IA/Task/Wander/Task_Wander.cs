using UnityEngine;
using System.Collections.Generic;

public class Task_Wander : AgentTaskBase
{
    private float _radius;
    private Vector3 _targetPosition;
    private bool _isFinished = false;
    private Transform _agentTransform;

    private float _scanCooldown;
    private float _scanTimer;
    private float _flowerDetectionRadius;
    private PathFinding _pathFinder = new PathFinding();
    private List<Vector3> _currentPath;
    private int _pathIndex = 0;
    private float _moveSpeed = 2f;

    public Task_Wander(string taskName, Blackboard bb, float radius, float scanCooldown, float scanTimer, float flowerDetectionRadius)
        : base(taskName, bb)
    {
        _radius = radius;
        _scanCooldown = scanCooldown;
        _scanTimer = scanTimer;
        _flowerDetectionRadius = flowerDetectionRadius;

        _agentTransform = (Transform)bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        _isFinished = false;

        Vector2 rnd = Random.insideUnitCircle * _radius;
        Vector3 rawTarget = _agentTransform.position + new Vector3(rnd.x, 0, rnd.y);

        Vector2Int targetCell = MapGenerator.Instance.WorldToGrid(rawTarget);
        _targetPosition = MapGenerator.Instance.GridToWorld(targetCell);

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
        if (_scanTimer >= _scanCooldown)
        {
            _scanTimer = 0f;
            ScanForFlowers();
        }
    }

    private void FollowPath()
    {
        if (_isFinished || _currentPath == null || _pathIndex >= _currentPath.Count)
            return;

        Vector3 target = _currentPath[_pathIndex];

        _agentTransform.position = Vector3.MoveTowards(
            _agentTransform.position,
            target,
            _moveSpeed * Time.deltaTime
        );

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

    public void ScanForFlowers()
    {
        Collider[] hits = Physics.OverlapSphere(_agentTransform.position, _flowerDetectionRadius);

        foreach (var hit in hits)
        {
            Flower flower = hit.GetComponentInParent<Flower>(); // à changer quand le système de spawn des fleurs sera en place
            if (flower != null && flower.ContainsPollen())
            {
                _bb.ModifyValue("TargetFlower", flower);
                return;
            }
        }
    }

    public override void OnFinish()
    {
        if (_isFinished)
        {
            _isFinished = false;
            OnStart();
        }
    }

    public override void OnCancel() { }

    public override float GetUtility()
    {
        bool hasFlower = _bb.GetValue("TargetFlower") != null;
        int collectedPollen = (int)_bb.GetValue("CollectedPollen");

        float cond_NoFlower = hasFlower ? 0f : 1f;
        float cond_NoPollen = collectedPollen > 0 ? 0f : 1f;

        Hunger h = GetHunger();
        Tiredness t = GetTiredness();

        float cond_NotHungry = Normalize(h._currentHungerValue, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t._currentTirednessValue, t._minTirednessValue, t._maxTirednessValue);

        return Combine(cond_NoPollen, cond_NoFlower, cond_NotHungry, cond_NotTired);
    }

    public override int GetTaskPriority() => 0;

    public override bool IsTaskFinished() => _isFinished;
}
