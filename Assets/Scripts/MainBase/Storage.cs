using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;

    private int _storageCapacity = 10;

    public bool IsStorageFull { get; private set; }
    public int CollectedRockCount { get; private set; }

    public event Action CollectedRock;

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