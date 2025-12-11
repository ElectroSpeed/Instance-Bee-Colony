using UnityEngine;
using System.Collections.Generic;

public class Task_Rest : AgentTaskBase
{
    private Transform _agentTransform;
    private Beehive _hive;
    private float _moveSpeed = 2.5f;
    private PathFinding _pathFinder = new PathFinding();
    private List<Vector3> _currentPath;
    private int _pathIndex = 0;
    private bool _isFinished = false;
    private bool _arrived = false;

    private float _restDuration;
    private float _restTimer;

    private AgentTaskBase _savedTask;

    public Task_Rest(string taskName, Blackboard bb, float restDuration) : base(taskName, bb)
    {
        _agentTransform = (Transform)_bb.GetValue("AgentTransform");
        _hive = (Beehive)_bb.GetValue("Hive");
        _restDuration = restDuration;
    }

    public override void OnStart()
    {
        if (_hive == null)
        {
            _isFinished = true;
            return;
        }

        _isFinished = false;
        _arrived = false;
        _restTimer = 0f;

        AgentTaskBase current = (AgentTaskBase)_bb.GetValue("CurrentTask");
        if (current != null && !(current is Task_Rest))
            _savedTask = current;
        else
            _savedTask = null;

        Vector3 target = _hive.transform.position;
        _currentPath = _pathFinder.FindPathPositions(_agentTransform.position, target);
        _pathIndex = 0;

        if (_currentPath == null || _currentPath.Count == 0)
            _arrived = true;
    }

    public override void OnUpdate()
    {
        if (_isFinished) return;

        if (!_arrived)
        {
            FollowPath();
        }
        else
        {
            _restTimer += Time.deltaTime;

            if (_restTimer >= _restDuration)
            {
                Hunger h = GetHunger();
                Tiredness t = GetTiredness();

                h.Current = Mathf.Min(h._minHungerValue, h.Current + 50f);
                t.Current = Mathf.Min(t._minTirednessValue, t.Current + 50f);

                _bb.ModifyValue("Hunger", h);
                _bb.ModifyValue("Tiredness", t);

                _isFinished = true;

                if (_savedTask != null)
                {
                    _savedTask.OnStart();
                    _bb.ModifyValue("CurrentTask", _savedTask);
                }
                else
                {
                    _bb.ModifyValue("CurrentTask", null);
                }
            }
        }
    }

    private void FollowPath()
    {
        if (_currentPath == null || _pathIndex >= _currentPath.Count)
        {
            _arrived = true;
            return;
        }

        Vector3 target = _currentPath[_pathIndex];
        _agentTransform.position = Vector3.MoveTowards(_agentTransform.position, target, _moveSpeed * Time.deltaTime);

        Vector3 direction = (target - _agentTransform.position);
        if (direction.sqrMagnitude > 0.01f)
            _agentTransform.forward = direction.normalized;

        if (Vector3.Distance(_agentTransform.position, target) < 0.1f)
            _pathIndex++;
    }

    public override void OnFinish() { }
    public override void OnCancel() { _isFinished = true; }

    public override float GetUtility()
    {
        Hunger h = GetHunger();
        Tiredness t = GetTiredness();

        float hungerThreshold = h._maxHungerValue - 20f;
        float tiredThreshold = t._maxTirednessValue - 20f;

        if (h.Current >= hungerThreshold || t.Current >= tiredThreshold)
            return 1f;

        return 0f;
    }

    public override int GetTaskPriority() => 2;

    public override bool IsTaskFinished() => _isFinished;
}
