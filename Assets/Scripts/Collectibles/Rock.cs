using System;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public event Action<Rock> Collected;

    public Transform SpawnPoint;

    public void InvokeReturnToPool()
    {
        Collected.Invoke(this);
    }
}