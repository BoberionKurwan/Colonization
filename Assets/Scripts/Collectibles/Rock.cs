using System;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public event Action<Rock> ReturnToPool;

    public Transform SpawnPoint;

    public void InvokeReturnToPool()
    {
        ReturnToPool.Invoke(this);
    }
}