using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PathFinding))]
public class Agent : MonoBehaviour
{
    [Header("Agent need Parameters")]
    [SerializeField] private List<TaskDataBase> _tasksData;

    private List<AgentTaskBase> _agentsTask = new();
    private AgentTaskBase _currentTask;
    public Blackboard _bb;

    public PathFinding _pathfinder;



    public void Start()
    {
        Initialize();
        CreateAgentTasks();
    }

    public virtual void Initialize()
    {
        _pathfinder = GetComponent<PathFinding>();
        if (_pathfinder == null) _pathfinder = gameObject.AddComponent<PathFinding>();
    }

    public Blackboard GetBlackboard() => _bb;

    public void CreateAgentTasks()
    {
        foreach (var taskData in _tasksData)
        {
            AgentTaskBase newTask = taskData.CreateTaskInstance(_bb, this);

            _agentsTask.Add(newTask);
        }
    }

    protected virtual void Update()
    {
        AgentTaskBase bestTask = GetBestTask();
        if (bestTask != _currentTask)
        {
            if (_currentTask != null)
                _currentTask.OnCancel();

            _currentTask = bestTask;
            _currentTask?.OnStart();
        }
        _currentTask?.OnUpdate();
    }

    private AgentTaskBase GetBestTask()
    {
        AgentTaskBase bestTask = null;
        float highestPriority = float.NegativeInfinity;

        bool force = (bool)(_bb.GetValue("ForceRecalculate") ?? false);
        if (force)
            _bb.ModifyValue("ForceRecalculate", false);

        foreach (AgentTaskBase task in _agentsTask)
        {
            float priority = task.GetUtility();

            if (priority > highestPriority)
            {
                highestPriority = priority;
                bestTask = task;
            }
        }
        return bestTask;
    }

    public void StartGetPathPoint(Vector3 startWorld, Vector3 endWorld, Action<List<Vector3>> callback)
    {
        StartCoroutine(_pathfinder.FindPathPositions(startWorld, endWorld, result =>
        {
            if (result.Count > 0)
            {
                callback?.Invoke(result);
            }
        }));
    }
}
