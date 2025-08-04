using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPointsBuffer = new List<Transform>();

    private List<Transform> _emptySpawnPoints = new List<Transform>();
    private List<Transform> _busySpawnPoints = new List<Transform>();

    public int GetEmptyCount() => _emptySpawnPoints.Count;

    private void Awake()
    {
        _emptySpawnPoints = new List<Transform>(_spawnPointsBuffer);
    }

    public bool TryGetRandomEmptyPoint(out Transform point)
    {
        if (_emptySpawnPoints.Count != 0)
        {
            int randomIndex = Random.Range(0, _emptySpawnPoints.Count);
            point = _emptySpawnPoints[randomIndex];
            _busySpawnPoints.Add(_emptySpawnPoints[randomIndex]);
            _emptySpawnPoints.RemoveAt(randomIndex);
            return true;
        }
        else
        {
            point = null;
            return false;
        }
    }

    public void ReleaseSpawnPoint(Transform point)
    {
        _emptySpawnPoints.Add(point);
        _busySpawnPoints.Remove(point);
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Child Array")]
    private void RefreshChildArray()
    {
        _spawnPointsBuffer.Clear();

        foreach (Transform child in transform)
            _spawnPointsBuffer.Add(child);
    }
#endif
}