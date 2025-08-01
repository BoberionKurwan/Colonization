using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private int _storageCapacity = 10;

    public event Action CollectedRock;

    public bool IsStorageFull { get; private set; }
    public int CollectedRockCount { get; private set; }

    public void StoreRock(Rock rock)
    {
        if (CollectedRockCount < _storageCapacity)
        {
            CollectedRockCount++;
            rock.InvokeReturnToPool();
            CollectedRock?.Invoke();
        }
        else
        {
            IsStorageFull = true;
        }
    }
}