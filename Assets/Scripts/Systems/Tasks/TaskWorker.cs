using System;
using UnityEngine;

public abstract class TaskWorker : MonoBehaviour
{
    public abstract bool IsValidTask(Task task);

    public abstract void MoveTo(Vector3 position, Action onArrivedAtPosition);
}
