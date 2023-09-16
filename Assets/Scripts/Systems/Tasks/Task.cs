using UnityEngine;

public class Task
{
    public Vector3 targetPosition;

    // priority tasks are executed immediately and are not managed through the task system
    // if a priority task is cancelled it does not return to the task queue
    public bool isPriorityTask;

    public Task(Vector3 targetPosition, bool isPriorityTask)
    {
        this.targetPosition = targetPosition;
        this.isPriorityTask = isPriorityTask;
    }
}
