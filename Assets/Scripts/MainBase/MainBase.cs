using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Scanner), typeof(Storage), typeof(WorkerSpawner))]
[RequireComponent(typeof(InputReader))]
public class MainBase : MonoBehaviour
{
    [SerializeField] private int _spawnWorkerCount = 3;
    [SerializeField] private BotRetriever _botRetriever;

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
        _inputReader.SendWorkersToCollect += SendWorkersToCollect;
        _inputReader.ScanForResourses += ScanForRocks;
        _botRetriever.WorkerEntered += OnRockDelivered;

        _rocks = _scanner.GetRocks();

        for (int i = 0; i < _spawnWorkerCount; i++)
            _workers.Add(_workerSpawner.Spawn());

    }

    private void OnDestroy()
    {
        _inputReader.SendWorkersToCollect -= SendWorkersToCollect;
        _inputReader.ScanForResourses -= ScanForRocks;
        _botRetriever.WorkerEntered -= OnRockDelivered;
    }

    private void ScanForRocks()
    {
        _rocks = _scanner.GetRocks();
    }

    private void SendWorkersToCollect()
    {
        int count = Mathf.Min(_workers.Count, _rocks.Count);

        for (int i = 0; i < count; i++)
        {
            if (_workers[i].IsFree)
            {
                _workers[i].IsFree = false;
                _workers[i].SetTarget(_rocks[i]);
                _workers[i].SetStorage(_botRetriever.transform);
                _rocks.RemoveAt(i);
            }
        }
    }

    private void OnRockDelivered(Worker worker)
    {
        Rock rock = worker.GiveRock();

        if (rock != null)
            _storage.StoreRock(rock);
    }
}