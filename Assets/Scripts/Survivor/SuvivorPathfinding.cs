using Pathfinding;
using UnityEngine;

public class SurvivorPathfinding : MonoBehaviour
{

    // TODO: targets for pathfinding should be event-driven

    private AIPath aiPath;

    private Vector3 targetPosition;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
    }
}
