using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private int _storageCapacity = 10;

    public event Action CountChanged;
    
    public bool IsStorageFull { get; private set; }
    public int CollectedCount { get; private set; }

    public void StoreRock(Rock rock)
    {
        if (CollectedCount < _storageCapacity)
        {
            CollectedCount++;
            rock.InvokeReturnToPool();
            CountChanged?.Invoke();
        }
        else
        {
            IsStorageFull = true;
        }        
    }

    public void SpendResources(int count)
    {
        CollectedCount -= count;
        CountChanged?.Invoke();
    }
}