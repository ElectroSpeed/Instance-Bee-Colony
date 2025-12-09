using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    [Header("Agent need Parameters")]
    [SerializeField] private AgentStatData _statsData;
    [SerializeField] private List<TaskDataBase> _tasksData;


    private List<AgentTaskBase> _agentsTask = new(); 
    private AgentTaskBase _currentTask;
    private Blackboard _bb; 

    void Start()
    {
        _bb = new Blackboard();
        _bb.AddValue("AgentTransform", this.transform);
        _bb.AddValue("TargetFlower", null);
        CreateAgentTasks();
    }

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
