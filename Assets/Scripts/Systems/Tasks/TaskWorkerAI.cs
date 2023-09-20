using UnityEngine;

public class TaskWorkerAI : MonoBehaviour
{
    [SerializeField]
    private TaskWorker worker;

    private enum State
    {
        Idle,
        Working
    }

    private State state;

    private float idleTimer;

    private Task taskInProgress;

    private void Start()
    {
        state = State.Idle;
    }

    public void SetTask(Task task)
    {
        if (worker.IsValidTask(task))
        {
            CancelTask();
            ExecuteTask(task);
        }
    }

    private void Update()
    {
        if (state == State.Idle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                // request next task after 200ms idle
                float idleTimerMax = 0.2f;
                idleTimer = idleTimerMax;
                RequestNextTask();
            }
        }
    }

    private void RequestNextTask()
    {
        // TODO: pass task validation function to the task system
        Task task = TaskSystem.Instance.RequestNextTask();
        if (task == null)
        {
            return;
        }

        if (worker.IsValidTask(task))
        {
            ExecuteTask(task);
        }
    }

    private void ExecuteTask(Task task)
    {
        switch (task)
        {
            case MoveTask moveTask:
                ExecuteMoveTask(moveTask);
                break;
            default:
                Debug.LogError(string.Format("Attempted to execute unknown task type: {0}", task));
                break;
        }
    }

    private void ExecuteMoveTask(MoveTask moveTask)
    {
        taskInProgress = moveTask;
        state = State.Working;
        worker.MoveTo(moveTask.GetTargetPosition(), () =>
        {
            taskInProgress = null;
            state = State.Idle;
        });
    }

    private void CancelTask()
    {
        if (taskInProgress != null && !taskInProgress.GetIsPriorityTask())
        {
            // return the task to the task queue
            TaskSystem.Instance.AddTask(taskInProgress, true);
        }

        taskInProgress = null;
        state = State.Idle;
    }
}
