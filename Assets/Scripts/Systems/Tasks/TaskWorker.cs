using System;
using UnityEngine;

public abstract class TaskWorker : MonoBehaviour
{
    public abstract void MoveTo(Vector3 position, Action onArrivedAtPosition = null);
}
