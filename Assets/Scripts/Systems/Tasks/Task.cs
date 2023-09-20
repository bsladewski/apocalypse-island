using UnityEngine;

public class Task
{
    private Vector3 targetPosition;

    // priority tasks are executed immediately by a specific worker and are not managed through the
    // task system. if a priority task is cancelled it does not return to the task queue
    private bool isPriorityTask;

    public Task(Vector3 targetPosition, bool isPriorityTask)
    {
        this.targetPosition = targetPosition;
        this.isPriorityTask = isPriorityTask;
    }

    public Vector3 GetTargetPosition()
    {
        return targetPosition;
    }

    public bool GetIsPriorityTask()
    {
        return isPriorityTask;
    }
}
