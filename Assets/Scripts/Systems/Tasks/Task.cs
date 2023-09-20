using System.Collections.Generic;
using UnityEngine;

public class Task
{
    // priority tasks are executed immediately by a specific worker and are not managed through the
    // task system. if a priority task is cancelled it does not return to the task queue
    protected bool isPriorityTask;

    public Task() : this(false) { }

    public Task(bool isPriorityTask)
    {
        this.isPriorityTask = isPriorityTask;
    }

    public bool GetIsPriorityTask()
    {
        return isPriorityTask;
    }
}

public class MoveTask : Task
{
    private Vector3 targetPosition;

    public MoveTask(Vector3 targetPosition) : this(targetPosition, false) { }

    public MoveTask(Vector3 targetPosition, bool isPriorityTask)
    {
        this.targetPosition = targetPosition;
        this.isPriorityTask = isPriorityTask;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }
}

public class TaskGroup : Task
{
    private List<Task> subTaskList;

    public TaskGroup() : this(false) { }

    public TaskGroup(bool isPriorityTask)
    {
        subTaskList = new List<Task>();
        this.isPriorityTask = isPriorityTask;
    }

    public void AddTask(Task task)
    {
        subTaskList.Add(task);
    }

    public Task GetCurrentTask()
    {
        if (subTaskList.Count > 0)
        {
            return subTaskList[0];
        }
        return null;
    }

    public void RemoveCurrentTask()
    {
        if (subTaskList.Count > 0)
        {
            subTaskList.RemoveAt(0);
        }
    }
}
