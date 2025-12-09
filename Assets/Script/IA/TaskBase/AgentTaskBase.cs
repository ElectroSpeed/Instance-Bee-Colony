using UnityEngine;

public abstract class AgentTaskBase
{
    [Header ("Global Task Var")]
    [SerializeField] protected string _taskName;
    protected Blackboard _bb;

    /// <summary>
    /// AgentTask Constructor Override this when the Task need other parameters in children class.
    /// </summary>
    
    public AgentTaskBase(string taskName, Blackboard bb)
    {
        this._taskName = taskName;
        this._bb = bb;
    }

    #region CallBack Functions

    /// <summary>
    /// Called when the task begins execution.
    /// Initialize variables or start required processes here.
    /// </summary>
    public abstract void OnStart();

    /// <summary>
    /// Called every update tick while the task is running.
    /// Contains the main logic executed continuously until the task ends. 
    /// </summary>
    public abstract void OnUpdate();

    /// <summary>
    /// Called when the task successfully completes its operation.
    /// Use this to clean up or trigger follow-up actions.
    /// </summary>
    public abstract void OnFinish();

    /// <summary>
    /// Called when the task is interrupted or cancelled before completion.
    /// Use this to revert partial changes or stop on going operations.
    /// </summary>
    public abstract void OnCancel();

    #endregion

    #region Utility Functions

    /// <summary>
    /// Returns whether the task is currently allowed to execute.
    /// Override this to add preconditions or context validations.
    /// </summary>
    public virtual bool CanDo() => true;

    /// <summary>
    /// Computes the utility score determining how desirable this task is.
    /// Higher values increase the likelihood of this task being selected.
    /// </summary>
    public abstract float GetUtility(); // (ancien ComputeUtility)

    /// <summary>
    /// Returns the priority level of the task.
    /// Useful when task have similar utility and must be ordered deterministically.
    /// </summary>
    public abstract int GetTaskPriority();

    /// <summary>
    /// Indicates whether the task has finished running and can be stopped.
    /// </summary>
    public abstract bool IsTaskFinished();

    #endregion
}
