using System;
using UnityEngine;

public class BotRetriever : MonoBehaviour
{
    public event Action<Worker> WorkerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Worker worker))
        {
            WorkerEntered?.Invoke(worker);
        }
    }
}
