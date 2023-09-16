using System;
using System.Collections;
using Pathfinding;
using UnityEngine;

public class SurvivorWorker : TaskWorker
{
    [SerializeField]
    SurvivorPathfinding survivorPathfinding;

    private IEnumerator awaitPathComplete;

    public override void MoveTo(Vector3 targetPosition, Action onArrivedAtPosition)
    {
        if (awaitPathComplete != null)
        {
            StopCoroutine(awaitPathComplete);
        }

        // calculate the closest point the survivor can move to the target position
        NNInfo survivorNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
        NNConstraint constraint = NNConstraint.Default;
        constraint.constrainArea = true;
        constraint.area = (int)survivorNode.node.Area;
        NNInfo closestNode = AstarPath.active.GetNearest(targetPosition, constraint);

        // set the target position for the survivor pathfinder
        survivorPathfinding.SetTargetPosition(closestNode.position);
        awaitPathComplete = AwaitPathComplete(onArrivedAtPosition);
        StartCoroutine(awaitPathComplete);
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
