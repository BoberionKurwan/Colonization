using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace MainBase
{
    [RequireComponent(typeof(Storage), typeof(WorkerSpawner), typeof(WorkerRetriever))]
    [RequireComponent(typeof(FlagPlacer), typeof(WorkerForeman))]
    public class MainBase : MonoBehaviour
    {
        [SerializeField] private ResourcesRepository _resourcesRepository;
        [SerializeField] private BaseBuilder _baseBuilder;
        [SerializeField] private List<Worker> _workers = new List<Worker>();

        private Storage _storage;
        private WorkerSpawner _workerSpawner;
        private WorkerRetriever _botRetriever;
        private FlagPlacer _flagPlacer;
        private WorkerForeman _workerForeman;

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
            _workerForeman = GetComponent<WorkerForeman>();
        }

        private void OnEnable()
        {
            _botRetriever.WorkerEntered += OnRockDelivered;
            _flagPlacer.WorkerCame += BuildBase;
            StartCoroutine(CollectResourсes());
        }

        private void OnDisable()
        {
            _botRetriever.WorkerEntered -= OnRockDelivered;
            _flagPlacer.WorkerCame -= BuildBase;
        }

        public void Initialize(Worker worker, ResourcesRepository resourcesRepository, BaseBuilder baseBuilder)
        {
            AddWorker(worker);
            _resourcesRepository = resourcesRepository;
            _baseBuilder = baseBuilder;

            if (_resourcesRepository.TryGetRock(out Rock rock))
            {
                worker.SetTarget(rock.transform);
            }
        }

        public void SpawnFlag(Vector3 position)
        {
            _flagPlacer.SetPosition(position);
        }

        public ResourcesRepository GetRepository()
        {
            return _resourcesRepository;
        }

        private void AddWorker(Worker worker)
        {
            _workers.Add(worker);
            worker.SetStorage(_storage.transform);
        }

        private void BuildBase(Worker worker)
        {
            _storage.SpendResources(_basePrice);
            BuildingBase?.Invoke(_flagPlacer.GetFlagPosition(), worker);
            _workers.Remove(worker);
        }

        private void OnRockDelivered(Worker worker)
        {
            if (_workers.Contains(worker) == false)
            {
                return;
            }

            Rock rock = worker.GetRock();
            _storage.StoreRock(rock);
            RockDelivered?.Invoke(rock);

            if (_flagPlacer.IsFlagActive && _storage.CollectedCount >= _basePrice && _workers.Count > _workerMinCount)
            {
                Worker freeWorker = _workers.FirstOrDefault(w => w.IsFree);
                _workerForeman.SendWorkerToBuild(freeWorker, _flagPlacer.GetFlagPosition());
                _flagPlacer.SetWorker(freeWorker);
            }
            else if (_storage.CollectedCount >= _workerPrice)
            {
                _storage.SpendResources(_workerPrice);
                _workers.Add(_workerSpawner.BuildWorker(_botRetriever.transform));
            }
            else if (_resourcesRepository.TryGetRock(out Rock targetRock))
            {
                Transform target = targetRock.transform;
                _workerForeman.SendWorkersToCollect(_workers, _botRetriever.transform, target);
            }
        }

        private IEnumerator CollectResourсes()
        {
            WaitForSeconds delay = new WaitForSeconds(_delay);

            while (enabled)
            {
                if (_resourcesRepository.TryGetRock(out Rock targetRock))
                {
                    Transform target = targetRock.transform;
                    _workerForeman.SendWorkersToCollect(_workers, _botRetriever.transform, target);
                }

                yield return delay;
            }
        }
    }
}