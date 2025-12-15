using System.Globalization;
using UnityEngine;

public abstract class TaskDataBase : ScriptableObject
{
    public string _taskName;
    public abstract AgentTaskBase CreateTaskInstance(Blackboard bb);
}


public abstract class TaskDataBase<T> : TaskDataBase where T : AgentTaskBase
{
    protected abstract T CreateTypedTask(string taskName, Blackboard bb);
    public override AgentTaskBase CreateTaskInstance(Blackboard bb)
    {
        return CreateTypedTask(_taskName, bb);
    }
}