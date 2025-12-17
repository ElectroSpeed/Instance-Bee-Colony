using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Task_Wander : AgentTaskBase
{
    private float _radius;
    private Vector3 _targetPosition;
    private bool _isFinished = false;
    private Transform _agentTransform;

    private float _scanCooldown;
    private float _scanTimer;
    private float _flowerDetectionRadius;

    private List<Vector3> _currentPath;
    private int _pathIndex = 0;
    private float _moveSpeed = 2f;

    private bool _pathIsCalculated = false;

    public Task_Wander(string taskName, Blackboard bb, Agent agent, float radius, float scanCooldown, float scanTimer, float flowerDetectionRadius) : base(taskName, bb, agent)
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
        _pathIsCalculated = false;
        _currentPath = null;
        _pathIndex = 0;

        Vector3 rawTarget = GetValidWanderTarget();

        Vector2Int targetCell = MapGenerator.Instance.WorldToGrid(rawTarget);
        _targetPosition = MapGenerator.Instance.GridToWorld(targetCell);

        Debug.Log("Try call MoveAgent");
        _agent.StartGetPathPoint(
            _agentTransform.position,
            _targetPosition, result =>
            OnPathReady(result)
        );
    }
    private void OnPathReady(List<Vector3> result)
    {
        Debug.Log($"set path : {result.Count}");
        if (result == null || result.Count <= 0)
        {
            _isFinished = true;
            OnFinish();
            return;
        }

        _currentPath = result;
        _pathIndex = 0;
        _pathIsCalculated = true;
    }

    private Vector3 GetValidWanderTarget() // function to get a random point inside the hive's exploration area
    {
        Vector3 candidate;
        int safety = 0;

        do
        {
            Vector2 rnd = Random.insideUnitCircle * _radius;
            candidate = _agentTransform.position + new Vector3(rnd.x, 0, rnd.y);

            if (++safety > 20)
            {
                return _agentTransform.position;
            }

        }
        while (!IsInsideHiveZone(candidate));

        return candidate;
    }

    public override void OnUpdate()
    {
        if (!_pathIsCalculated) return;

        FollowPath();

        _scanTimer += Time.deltaTime;
        if (_scanTimer >= _scanCooldown)
        {
            _scanTimer = 0f;
            ScanForFlowers();
        }
    }

    private void FollowPath() // simple path following logic
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

    public void ScanForFlowers() // scan for flowers within detection radius
    {
        Collider[] hits = Physics.OverlapSphere(_agentTransform.position, _flowerDetectionRadius);

        foreach (var hit in hits)
        {
            Flower flower = hit.GetComponentInParent<Flower>();
            if (flower != null && flower.ContainsPollen())
            {
                if (!IsInsideHiveZone(flower.transform.position))
                    continue;

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
            _agent.TaskCanceled();
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

        float cond_NotHungry = Normalize(h.Current, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t.Current, t._minTirednessValue, t._maxTirednessValue);

        return Combine(cond_NoPollen, cond_NoFlower, cond_NotHungry, cond_NotTired);
    }

    public override int GetTaskPriority() => 0;

    public override bool IsTaskFinished() => _isFinished;
}
