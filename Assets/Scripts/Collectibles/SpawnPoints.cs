using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    private List<Transform> _emptySpawnPoints = new List<Transform>();
    private List<Transform> _busySpawnPoints = new List<Transform>();

    private void Awake()
    {
        RefreshChildArray();
    }

    public Transform GetRandomEmptyPoint()
    {
        int randomIndex = Random.Range(0, _emptySpawnPoints.Count);
        Transform point = _emptySpawnPoints[randomIndex];
        _busySpawnPoints.Add(_emptySpawnPoints[randomIndex]);
        _emptySpawnPoints.RemoveAt(randomIndex);
        return point;
    }

    public void ClearBusyPoint(Transform point)
    {
        _emptySpawnPoints.Add(point);
        _busySpawnPoints.Remove(point);
    }

    public int GetEmptyCount()
    {
        return _emptySpawnPoints.Count;
    }

    private void RefreshChildArray()
    {
        _emptySpawnPoints.Clear();

        foreach (Transform child in transform)
            _emptySpawnPoints.Add(child);
    }
}