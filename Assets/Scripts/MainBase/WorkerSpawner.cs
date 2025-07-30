using UnityEngine;
using System.Collections;
using System;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] protected Worker _prefab;
    [SerializeField] protected Transform _spawnPoint;
    [SerializeField] private float _delay = 2f;

    private Coroutine _spawnCoroutine;
    private int _spawnedCount = 0;
    private int _spawnCount = 3;

    public event Action<Worker> WorkerSpawned;

    public void Spawn(int count)
    {
        _spawnCount = count;
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private void OnDestroy()
    {
        StopCoroutine(_spawnCoroutine);
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (_spawnedCount < _spawnCount)
        {
            WorkerSpawned?.Invoke(Instantiate(_prefab, _spawnPoint.position, transform.rotation));
            _spawnedCount++;
            yield return delay;
        }

        StopCoroutine(_spawnCoroutine);
    }
}
