using System.Globalization;
using UnityEngine;

public abstract class TaskDataBase : ScriptableObject
{
    public string _taskName;
    public abstract AgentTaskBase CreateTaskInstance(Blackboard bb, Agent agent);
}


public abstract class TaskDataBase<T> : TaskDataBase where T : AgentTaskBase
{
    protected abstract T CreateTypedTask(string taskName, Blackboard bb, Agent agent);
    public override AgentTaskBase CreateTaskInstance(Blackboard bb, Agent agent)
    {
        return CreateTypedTask(_taskName, bb, agent);
    }
}