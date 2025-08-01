using System;
using UnityEngine;

public class BotRetriever : MonoBehaviour
{
    public event Action<Worker> WorkerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Worker worker) && worker.IsCarryingResource)
        {
            WorkerEntered?.Invoke(worker);
        }
    }
}
