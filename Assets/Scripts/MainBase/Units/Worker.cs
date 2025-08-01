using UnityEngine;

[RequireComponent(typeof(WorkerMover), typeof(ResourceCollector))]
public class Worker : MonoBehaviour
{
    private WorkerMover _workerMover;
    private ResourceCollector _resourceCollector;

    private Transform _targetPosition;
    private Transform _storagePoint;

    public bool IsFree;

    private void Awake()
    {
        _workerMover = GetComponent<WorkerMover>();
        _resourceCollector = GetComponent<ResourceCollector>();
    }

    private void Start()
    {
        _resourceCollector.Collected += OnCollected;
    }

    private void FixedUpdate()
    {
        if (_targetPosition.position != null)
        {
            _workerMover.MoveToTarget(_targetPosition.position);
        }
    }

    public void SetTarget(Rock rock)
    {
        _targetPosition = rock.transform;
        _resourceCollector.SetTarget(rock);
    }

    public void SetStorage(Transform storagePoint)
    {
        _storagePoint = storagePoint;
    }

    public Rock GiveRock()
    {
        IsFree = true;
        return _resourceCollector.GiveRock();
    }

    private void OnCollected()
    {
        _targetPosition = _storagePoint;
    }
}
