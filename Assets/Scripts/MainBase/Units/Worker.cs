using System;
using UnityEngine;

[RequireComponent(typeof(WorkerMover), typeof(WorkerTaker))]
public class Worker : MonoBehaviour
{
    [SerializeField] private float _distanceToTarget = 1f;

    private WorkerMover _workerMover;
    private WorkerTaker _workerTaker;

    private Rock _currentRock;
    private Transform _storagePoint;


    private WorkerState _state = WorkerState.Idle;

    public event Action<Rock> RockDelivered;

    public bool IsIdle() => _state == WorkerState.Idle;

    private enum WorkerState
    {
        Idle,
        MovingToRock,
        CarryingRock,
        MovingToStorage
    }

    private void Awake()
    {
        _workerMover = GetComponent<WorkerMover>();
        _workerTaker = GetComponent<WorkerTaker>();
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case WorkerState.MovingToRock:
                _workerMover.MoveToTarget(_currentRock.transform.position);

                if (IsTargetReached(_currentRock.transform.position))
                {
                    _workerTaker.PickUpRock(_currentRock);
                    _state = WorkerState.MovingToStorage;
                }

                break;

            case WorkerState.MovingToStorage:
                _workerMover.MoveToTarget(_storagePoint.position);

                if(IsTargetReached(_storagePoint.position))
                {
                    DeliverRock();
                }

                break;
        }
    }   

    public void SetTarget(Rock rock)
    {
        _currentRock = rock;
    }

    public void SetStorage(Transform storagePoint)
    {
        _storagePoint = storagePoint;
    }

    public void SetStateMovingToRock()
    {
        _state = WorkerState.MovingToRock;
    }

    private void DeliverRock()
    {
        if (_currentRock == null) return;

        RockDelivered?.Invoke(_currentRock);
        _currentRock.transform.SetParent(null);
        _currentRock = null;
        _state = WorkerState.Idle;
    }

    private bool IsTargetReached(Vector3 position)
    {
        return Vector3.Distance(transform.position, position) <= _distanceToTarget;
    }
}
