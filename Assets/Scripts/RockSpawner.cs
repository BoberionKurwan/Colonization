using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] protected Rock _prefab;
    [SerializeField] protected Transform[] _spawnPoints;

    private void Awake()
    {
        foreach (Transform point in _spawnPoints)
        {
            Instantiate(_prefab, point.transform.position, point.transform.rotation);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Refresh Child Array")]
    protected virtual void RefreshChildArray()
    {
        int pointCount = transform.childCount;
        _spawnPoints = new Transform[pointCount];

        for (int i = 0; i < pointCount; i++)
            _spawnPoints[i] = transform.GetChild(i);
    }
#endif
}