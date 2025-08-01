using UnityEngine;

public class WorkerSpawner : MonoBehaviour
{
    [SerializeField] protected Worker _prefab;
    [SerializeField] protected Transform _spawnPoint;

    public Worker Spawn()
    {
        return Instantiate(_prefab, _spawnPoint.position, transform.rotation);
    }
}
