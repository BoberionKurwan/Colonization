using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

[RequireComponent(typeof(Storage), typeof(WorkerSpawner), typeof(WorkerRetriever))]
[RequireComponent(typeof(FlagPlacer))]
public class MainBase : MonoBehaviour
{
    [SerializeField] private ResourcesRepository _resoucesRepository;
    [SerializeField] private BaseBuilder _baseBuilder;
    [SerializeField] private List<Worker> _workers = new List<Worker>();

    private Storage _storage;
    private WorkerSpawner _workerSpawner;
    private WorkerRetriever _botRetriever;
    private FlagPlacer _flagPlacer;

    private readonly int _workerMinCount = 1;
    private readonly int _workerPrice = 3;
    private readonly int _basePrice = 5;
    private readonly float _delay = 4;

    public event Action<Rock> RockDelivered;
    public event Action<Transform, Worker> BuildingBase;

    private void Awake()
    {
        _storage = GetComponent<Storage>();
        _workerSpawner = GetComponent<WorkerSpawner>();
        _botRetriever = GetComponent<WorkerRetriever>();
        _flagPlacer = GetComponent<FlagPlacer>();
    }

    private void OnEnable()
    {
        _botRetriever.WorkerEntered += OnRockDelivered;
        _flagPlacer.WorkerCame += BuildBase;
        StartCoroutine(CollectResourses());
    }

    private void OnDisable()
    {
        _botRetriever.WorkerEntered -= OnRockDelivered;
        _flagPlacer.WorkerCame -= BuildBase;
    }

    public void Initialize(Worker worker, ResourcesRepository resourcesRepository, BaseBuilder baseBuilder)
    {
        AddWorker(worker);
        _resoucesRepository = resourcesRepository;
        _baseBuilder = baseBuilder;

        if (_resoucesRepository.TryGetRock(out Rock rock))
        {
            worker.SetTarget(rock.transform);
        }
    }

    public void SpawnFlag(Vector3 position)
    {
        _flagPlacer.SetPosition(position);
    }

    public void AddWorker(Worker worker)
    {
        _workers.Add(worker);
        worker.SetStorage(_storage.transform);
    }

    public ResourcesRepository GetRepository()
    {
        return _resoucesRepository;
    }

    private void SendWorkersToCollect()
    {
        for (int i = 0; i < _workers.Count; i++)
        {
            if (_workers[i].StoragePoint == null)
                _workers[i].SetStorage(_botRetriever.transform);

            if (_workers[i].IsFree)
            {
                if (_resoucesRepository.TryGetRock(out Rock rock))
                {
                    _workers[i].SetTarget(rock.transform);
                }
            }
        }
    }

    private void SendWorkerToBuild()
    {
        if (!_flagPlacer.IsFlagActive) return;

        Worker worker = _workers.FirstOrDefault(worker => worker.IsFree);

        if (worker != null)
        {
            worker.SetTarget(_flagPlacer.GetFlagPosition());
            _flagPlacer.SetWorker(worker);
        }
    }

    private void BuildBase(Worker worker)
    {
        _storage.SpendResources(_basePrice);
        BuildingBase?.Invoke(_flagPlacer.GetFlagPosition(), worker);
        _workers.Remove(worker);
    }

    private void BuildWorker()
    {
        _storage.SpendResources(_workerPrice);

        Worker newWorker = _workerSpawner.Spawn();
        newWorker.SetStorage(_botRetriever.transform);
        _workers.Add(newWorker);
    }

    private void OnRockDelivered(Worker worker)
    {
        if (_workers.Contains(worker) == false)
        {
            return;
        }

        Rock rock = worker.GetRock();

        if (rock != null)
        {
            _storage.StoreRock(rock);
            RockDelivered?.Invoke(rock);
        }

        if (_flagPlacer.IsFlagActive)
        {
            if (_storage.CollectedCount >= _basePrice && _workers.Count > _workerMinCount)
            {
                SendWorkerToBuild();
            }
            else if (_storage.CollectedCount >= _workerPrice && _workers.Count == _workerMinCount)
            {
                BuildWorker();
            }
            else
            {
                SendWorkersToCollect();
            }
        }
        else if (_storage.CollectedCount >= _workerPrice)
        {
            BuildWorker();
        }
        else
        {
            SendWorkersToCollect();
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
