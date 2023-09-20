using UnityEngine;

public class SurvivorMoveInteraction : MonoBehaviour
{
    [SerializeField]
    private Selectable selectInteraction;

    [SerializeField]
    private TaskWorkerAI taskWorkerAI;

    private void Start()
    {
        InteractionSystem.Instance.OnMoveInteraction += OnMoveInteraction;
    }

    private void OnMoveInteraction(object sender, Vector3 movePosition)
    {
        if (selectInteraction.GetIsSelected())
        {
            taskWorkerAI.SetTask(new MoveTask(movePosition, true));
        }
    }
}
