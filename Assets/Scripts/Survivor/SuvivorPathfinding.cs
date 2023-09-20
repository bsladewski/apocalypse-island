using Pathfinding;
using UnityEngine;

public class SurvivorPathfinding : MonoBehaviour
{
    private AIPath aiPath;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
    }

    public Vector3? GetClosestDestination(Vector3 targetPosition)
    {
        float defaultPathfindingBuffer = 5f;
        return GetClosestDestination(targetPosition, defaultPathfindingBuffer);
    }

    public Vector3? GetClosestDestination(Vector3 targetPosition, float maxDistance)
    {
        // calculate the closest point the survivor can move to the target position
        NNInfo survivorNode = AstarPath.active.GetNearest(transform.position, NNConstraint.Default);
        NNConstraint constraint = NNConstraint.Default;
        constraint.constrainArea = true;
        constraint.area = (int)survivorNode.node.Area;
        NNInfo closestNode = AstarPath.active.GetNearest(targetPosition, constraint);

        if (Vector3.Distance(closestNode.position, targetPosition) < maxDistance)
        {
            return closestNode.position;
        }

        return null;
    }

    public bool IsValidDestination(Vector3 targetPosition)
    {
        Vector3? destination = GetClosestDestination(targetPosition);
        return destination.HasValue;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        aiPath.destination = targetPosition;
    }

    public bool IsMoving()
    {
        return aiPath.pathPending || !aiPath.reachedDestination;
    }
}
