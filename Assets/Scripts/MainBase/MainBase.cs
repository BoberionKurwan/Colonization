using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(InputReader), typeof(Storage), typeof(WorkerSpawner))]
public class MainBase : MonoBehaviour
{
    [SerializeField] private BotRetriever _botRetriever;
    [SerializeField] ResourcesRepository _resoucesRepository;

    private Storage _storage;
    private WorkerSpawner _workerSpawner;
    private Coroutine _collectCoroutine;

    private List<Worker> _workers = new List<Worker>();

    private int _spawnWorkerCount = 3;
    private int _workerPrice = 3;
    private float _delay = 4;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _workerSpawner = GetComponent<WorkerSpawner>();
    }

    private void Start()
    {
        _botRetriever.WorkerEntered += OnRockDelivered;

        _collectCoroutine = StartCoroutine(CollectResourses());

        for (int i = 0; i < _spawnWorkerCount; i++)
        {
            Worker worker = _workerSpawner.Spawn();
            worker.SetStorage(_botRetriever.transform);
            _workers.Add(worker);
        }
    }

    private void OnDestroy()
    {
        _botRetriever.WorkerEntered -= OnRockDelivered;
    }

    private void SendWorkersToCollect()
    {
        for (int i = 0; i < _workers.Count; i++)
        {
            if (_workers[i].IsFree)
            {
                if (_resoucesRepository.TryGetRock(out Rock rock))
                {
                    _workers[i].SetTarget(rock);
                }
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

            Worker newWorker = _workerSpawner.Spawn();
            newWorker.SetStorage(_botRetriever.transform);
            _workers.Add(worker);
        }
    }

    private IEnumerator CollectResourses()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            SendWorkersToCollect();

            yield return delay;
        }
    }
}