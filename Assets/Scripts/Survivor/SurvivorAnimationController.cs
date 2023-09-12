using Pathfinding;
using UnityEngine;

public class SurvivorAnimation : MonoBehaviour
{
    [SerializeField]
    private AIPath aiPath;

    [SerializeField]
    private Animator animator;

    private void Update()
    {
        animator.SetFloat("Velocity", aiPath.desiredVelocity.magnitude / aiPath.maxSpeed);
    }
}
