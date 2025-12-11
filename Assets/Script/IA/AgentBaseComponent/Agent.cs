using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    [Header("Agent need Parameters")]
    [SerializeField] private AgentStatsData _statsData;
    [SerializeField] private List<TaskDataBase> _tasksData;
    [SerializeField] private Beehive hive;

    [Header("Decay Parameters")]
    [SerializeField] private float hungerDecayAmount = 5f;
    [SerializeField] private float tirednessDecayAmount = 5f;

    private float statTimer = 0f;
    private float statInterval = 5f;


    private List<AgentTaskBase> _agentsTask = new(); 
    private AgentTaskBase _currentTask;
    public Blackboard _bb; 

    void Start()
    {
        _bb = new Blackboard();
        _bb.AddValue("AgentTransform", this.transform);

        _bb.AddValue("Hunger", new Hunger(
            _statsData._minHungerValue,
            _statsData._maxHungerValue,
            _statsData._currentHungerValue
        ));

        _bb.AddValue("Tiredness", new Tiredness(
            _statsData._minTirednessValue,
            _statsData._maxTirednessValue,
            _statsData._currentTirednessValue
        ));

        _bb.AddValue("TargetFlower", null);
        _bb.AddValue("CollectedPollen", 0);
        _bb.AddValue("Hive", hive);
        CreateAgentTasks();
    }

    public Blackboard GetBlackboard() => _bb;

    public void CreateAgentTasks()
    {
        foreach (var taskData in _tasksData)
        {
            AgentTaskBase newTask = taskData.CreateTaskInstance(_bb);
            _agentsTask.Add(newTask);
        }
    }

    void Update()
    {
        print(_currentTask);

        statTimer += Time.deltaTime;
        if (statTimer >= statInterval)
        {
            statTimer = 0f;

            Hunger h = (Hunger)_bb.GetValue("Hunger");
            Tiredness t = (Tiredness)_bb.GetValue("Tiredness");
            Debug.Log($"Before Decay - Hunger: {h.Current}, Tiredness: {t.Current}");

            h.Current += hungerDecayAmount;
            t.Current += tirednessDecayAmount;

            _bb.ModifyValue("Hunger", h);
            _bb.ModifyValue("Tiredness", t);
        }


        AgentTaskBase bestTask = GetBestTask();

        if(bestTask != _currentTask)
        {
            if(_currentTask != null)
            {
                _currentTask?.OnCancel();
            }
            
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
}
