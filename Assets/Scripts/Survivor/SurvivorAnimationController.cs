using Pathfinding;
using UnityEngine;

public class SurvivorAnimation : MonoBehaviour
{
    [SerializeField]
    private AIPath aiPath;

    [SerializeField]
    private Animator animator;

    private Vector3 lastPosition;

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 moveVector = new Vector3(lastPosition.x - transform.position.x, 0f,
            lastPosition.z - transform.position.z);
        animator.SetFloat("Velocity", moveVector.magnitude / Time.deltaTime / aiPath.maxSpeed);
        lastPosition = transform.position;
    }

    public void StartMiningAnimation()
    {
        animator.SetBool("IsMining", true);
    }

    public void StopMiningAnimation()
    {
        animator.SetBool("IsMining", false);
    }
}
