using Pathfinding;
using UnityEngine;

public class SurvivorPathfinding : MonoBehaviour
{
    private AIPath aiPath;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
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
