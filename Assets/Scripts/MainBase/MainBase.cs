using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Scanner), typeof(Storage), typeof(WorkerSpawner))]
[RequireComponent(typeof(InputReader))]
public class MainBase : MonoBehaviour
{
    [SerializeField] private int _spawnWorkerCount = 3;
    [SerializeField] private Transform _storageCollectionPoint;

    private Scanner _scanner;
    private Storage _storage;
    private WorkerSpawner _workerSpawner;
    private InputReader _inputReader;

    private List<Rock> _rocks;
    private List<Worker> _workers = new List<Worker>();

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _storage = GetComponent<Storage>();
        _workerSpawner = GetComponent<WorkerSpawner>();
        _inputReader = GetComponent<InputReader>();
    }

    private void Start()
    {
        _workerSpawner.WorkerSpawned += AddWorker;

        _rocks = _scanner.GetRocks();
        _workerSpawner.Spawn(_spawnWorkerCount);

        _inputReader.SpaceClicked += SetTasks;
        _inputReader.EClicked += ScanForRocks;
    }

    private void OnDestroy()
    {
        _inputReader.SpaceClicked -= SetTasks;
        _inputReader.EClicked -= ScanForRocks;
    }

    private void ScanForRocks()
    {
        _rocks = _scanner.GetRocks();
    }

    private void SetTasks()
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