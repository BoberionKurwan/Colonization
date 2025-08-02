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

    private List<Worker> _workers = new List<Worker>();

    private int _workerPrice = 3;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _storage = GetComponent<Storage>();
        _workerSpawner = GetComponent<WorkerSpawner>();
    }

    private void Start()
    {
        _botRetriever.WorkerEntered += OnRockDelivered;
        _scanner.TerrainScanned += SendWorkersToCollect;

        for (int i = 0; i < _spawnWorkerCount; i++)
            _workers.Add(_workerSpawner.Spawn());
    }

    private void OnDestroy()
    {
        _botRetriever.WorkerEntered -= OnRockDelivered;
        _scanner.TerrainScanned -= SendWorkersToCollect;
    }

    private void SendWorkersToCollect(List<Rock> _rocks)
    {
        List<Rock> rocksToCollect = new List<Rock>(_rocks);

        int count = Mathf.Min(_workers.Count, rocksToCollect.Count);

        for (int i = 0; i < count; i++)
        {
            if (_workers[i].IsFree)
            {
                _workers[i].SetTarget(rocksToCollect[0]);
                _workers[i].SetStorage(_botRetriever.transform);
                rocksToCollect.RemoveAt(0);
            }
        }
    } 

    private void OnRockDelivered(Worker worker)
    {
        Rock rock = worker.GiveRock();

        if (rock != null)
            _storage.StoreRock(rock);

        if (_storage.CollectedCount >= _workerPrice)
        {
            _storage.SpendResources(_workerPrice);
            _workers.Add(_workerSpawner.Spawn());
        }
    }
}