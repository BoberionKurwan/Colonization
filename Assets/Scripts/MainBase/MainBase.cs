using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Scaner), typeof(Storage), typeof(WorkerSpawner))]
public class MainBase : MonoBehaviour
{
    [SerializeField] private int _spawnWorkerCount = 3;
    [SerializeField] private Transform _storageCollectionPoint;

    private Scaner _scaner;
    private Storage _storage;
    private WorkerSpawner _workerSpawner;

    private List<Rock> _rocks;
    private List<Worker> _workers = new List<Worker>();

    private void Awake()
    {
        _scaner = GetComponent<Scaner>();
        _storage = GetComponent<Storage>();
        _workerSpawner = GetComponent<WorkerSpawner>();
    }

    private void Start()
    {
        _workerSpawner.WorkerSpawned += AddWorker;

        _rocks = _scaner.GetRocks();
        _workerSpawner.Spawn(_spawnWorkerCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetToWork();
        }
    }

    private void GetToWork()
    {
        int count = Mathf.Min(_workers.Count, _rocks.Count);

        for (int i = 0; i < _workers.Count; i++)
        { 
            _workers[i].SetTarget(_rocks[i]);
            _workers[i].SetStorage(_storageCollectionPoint);
            _workers[i].RockDelivered += OnRockDelivered;
            _workers[i].SetStateMovingToRock();
            _rocks.RemoveAt(i);
        }
    }

    private void AddWorker(Worker worker)
    {
        _workers.Add(worker);
    }

    private void OnRockDelivered(Rock rock, Worker worker)
    {
        _storage.StoreRock(rock);

        worker.RockDelivered -= OnRockDelivered;        
    }
}