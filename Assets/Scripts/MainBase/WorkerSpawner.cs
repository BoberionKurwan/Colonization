using UnityEngine;
using UnityEngine.Serialization;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] private Worker _prefab;
    [SerializeField] private Transform _spawnPoint;

    public Worker BuildWorker(Transform StorageTransform)
    {
        Worker newWorker = Spawn();
        newWorker.SetStorage(StorageTransform);
        return newWorker;
    }
    
    private Worker Spawn()
    {
        return Instantiate(_prefab, _spawnPoint.position, transform.rotation);
    }
}