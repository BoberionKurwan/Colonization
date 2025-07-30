using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] Transform _storageTransform;
    [SerializeField] private Vector3 _offset;

    private Vector3 _currentOffset = Vector3.zero;
    private int storageCapacity = 10;

    public bool IsStorageFull { get; private set; }
    public int CollectedRockCount { get; private set; }

    public event Action CollectedRock;

    public void StoreRock(Rock rock)
    {
        if (CollectedRockCount < storageCapacity)
        {
            _currentOffset += _offset;
            rock.transform.position = _storageTransform.position + _currentOffset;
            rock.transform.SetParent(this.transform);
            rock.enabled = false;

            CollectedRockCount++;
            CollectedRock?.Invoke();
        }
        else
        {
            IsStorageFull = true;
        }
    }
}