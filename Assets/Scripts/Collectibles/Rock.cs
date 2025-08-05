using System;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public event Action<Rock> Collected;

    public Transform SpawnPoint { get; private set; }

    public void SetSpawnPoint(Transform spawnPoint)
    {
        SpawnPoint = spawnPoint;
    }
    
    public void InvokeReturnToPool()
    {
        Collected.Invoke(this);
    }
}