using System.Collections.Generic;
using UnityEngine;

public class Task_ReturnToHive : AgentTaskBase
{
    private Transform _agentTransform;
    private bool _isFinished = false;
    private float _speed;

    private float _waitDuration = 1.5f;
    private float _timer = 0f;
    private bool _arrived = false;

    private List<Vector3> _currentPath;
    private int _pathIndex = 0;
    private bool _pathIsCalculated = false;
    public Task_ReturnToHive(string name, Blackboard bb, Agent agent, float speed)
        : base(name, bb, agent)
    {
        _speed = speed;
        _agentTransform = (Transform)bb.GetValue("AgentTransform");
    }

    public override void OnStart()
    {
        Beehive hive = (Beehive)_bb.GetValue("Hive");
        if (hive == null)
        {
            _isFinished = true;
            return;
        }
        _pathIsCalculated = false;
        _isFinished = false;
        _timer = 0f;
        _arrived = false;

        _pathIndex = 0;

        if (_currentPath == null || _currentPath.Count == 0)
        {
            _isFinished = true;
        }

        _agent.StartGetPathPoint(_agentTransform.position, hive.transform.position, OnPathReady);
    }

    private void OnPathReady(List<Vector3> result)
    {
        if (result == null || result.Count == 0)
        {
            _isFinished = true;
            OnFinish();
            return;
        }

        _currentPath = result;
        _pathIndex = 0;
        _pathIsCalculated = true;
    }

    public override void OnUpdate()
    {
        if (!_pathIsCalculated) return;

        Beehive hive = (Beehive)_bb.GetValue("Hive");
        if (hive == null)
        {
            _isFinished = true;
            return;
        }

        if (!_arrived)
        {
            FollowPath();
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

    private void FollowPath()
    {
        if (_currentPath == null || _pathIndex >= _currentPath.Count)
        {
            _arrived = true;
            _timer = 0f;
            return;
        }

        Vector3 target = _currentPath[_pathIndex];
        _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, target, _speed * Time.deltaTime);

        Vector3 dir = (target - _agentTransform.position);
        if (dir.sqrMagnitude > 0.01f)
            _agentTransform.forward = dir.normalized;

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
        {
            _pathIndex++;
            if (_pathIndex >= _currentPath.Count)
            {
                _arrived = true;
                _timer = 0f;
            }
        }
    }

    public override bool IsTaskFinished() => _isFinished;

    public override void OnFinish()
    {
    }
    public override void OnCancel() { }

    public override int GetTaskPriority() => 3;

    public override float GetUtility()
    {
        int pollen = (int)_bb.GetValue("CollectedPollen");
        if (pollen <= 0) return 0f;

        Hunger h = GetHunger();
        Tiredness t = GetTiredness();

        float cond_HasPollen = 1f;
        float cond_NotHungry = Normalize(h.Current, h._minHungerValue, h._maxHungerValue);
        float cond_NotTired = Normalize(t.Current, t._minTirednessValue, t._maxTirednessValue);

        return Combine(cond_HasPollen, cond_NotHungry, cond_NotTired);
    }
}
