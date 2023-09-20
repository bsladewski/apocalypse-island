using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class SurvivorWorker : TaskWorker
{
    [SerializeField]
    SurvivorPathfinding survivorPathfinding;

    private IEnumerator awaitPathComplete;

    public override bool IsValidTask(Task task)
    {
        return survivorPathfinding.IsValidDestination(task.GetTargetPosition());
    }

    public override void MoveTo(Vector3 targetPosition, Action onArrivedAtPosition)
    {
        if (awaitPathComplete != null)
        {
            StopCoroutine(awaitPathComplete);
        }

        Vector3? newPosition = survivorPathfinding.GetClosestDestination(targetPosition);
        if (newPosition.HasValue)
        {
            survivorPathfinding.SetTargetPosition(newPosition.Value);
            awaitPathComplete = AwaitPathComplete(onArrivedAtPosition);
            StartCoroutine(awaitPathComplete);
        }
    }

    private IEnumerator AwaitPathComplete(Action onArrivedAtPosition)
    {
        while (survivorPathfinding.IsMoving())
        {
            yield return null;
        }

        if (onArrivedAtPosition != null)
        {
            onArrivedAtPosition();
        }
    }
}
