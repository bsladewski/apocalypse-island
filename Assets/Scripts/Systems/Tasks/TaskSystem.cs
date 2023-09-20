using System.Collections.Generic;
using UnityEngine;

public class TaskSystem : MonoBehaviour
{
    public static TaskSystem Instance { get; private set; }

    private List<Task> taskList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Singleton TaskSystem already instantiated!");
        }
        taskList = new List<Task>();
        Instance = this;
    }

    public void AddTask(Task task)
    {
        AddTask(task, false);
    }

    public void AddTask(Task task, bool addToHead)
    {
        if (addToHead)
        {
            taskList.Insert(0, task);
        }
        else
        {
            taskList.Add(task);
        }
    }

    public Task RequestNextTask()
    {
        if (taskList.Count > 0)
        {
            Task task = taskList[0];
            taskList.RemoveAt(0);
            return task;
        }

        return null;
    }
}
