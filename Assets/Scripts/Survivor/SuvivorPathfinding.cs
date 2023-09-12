using Pathfinding;
using UnityEngine;

public class SurvivorPathfinding : MonoBehaviour
{
    private AIPath aiPath;

    [SerializeField]
    private Transform target;

    private Vector3 targetPosition;

    private void Start()
    {
        aiPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        if (targetPosition != target.transform.position)
        {
            targetPosition = target.transform.position;
            aiPath.destination = targetPosition;
        }
    }
}
