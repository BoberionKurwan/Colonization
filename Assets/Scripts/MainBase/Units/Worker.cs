using System.Collections;
using UnityEngine;

[RequireComponent(typeof(WorkerMover), typeof(ResourceCollector))]
public class Worker : MonoBehaviour
{
    private WorkerMover _workerMover;
    private ResourceCollector _resourceCollector;
    private Transform _target;
    private Coroutine _movingCoroutine;

    public Transform StoragePoint { get; private set; }
    public bool IsFree { get; private set; } = true;
    public bool IsCarryingResource { get; private set; }

    private void Awake()
    {
        _workerMover = GetComponent<WorkerMover>();
        _resourceCollector = GetComponent<ResourceCollector>();
    }

    private void Start()
    {
        _resourceCollector.Collected += OnCollected;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Flag flag) && other.transform == _target)
        {
            flag.InvokeTaken();
            _target = null;
            IsFree = true;
        }
    }

    public void SetTarget(Transform target)
    {
        IsFree = false;
        _target = target.transform;

        if (target.TryGetComponent<Rock>(out _))
        {
            _resourceCollector.SetTarget(target);
        }

        if (_movingCoroutine != null)
            StopCoroutine(_movingCoroutine);

        _movingCoroutine = StartCoroutine(MoveRoutine());
    }

    public void SetStorage(Transform storagePoint)
    {
        StoragePoint = storagePoint;
    }

    public Rock GiveRock()
    {
        IsFree = true;
        IsCarryingResource = false;
        StopCoroutine(_movingCoroutine);
        return _resourceCollector.GiveRock();
    }

    private void OnCollected()
    {
        IsCarryingResource = true;
        _target = StoragePoint;
        StopCoroutine(_movingCoroutine);
        _movingCoroutine = StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        while (enabled)
        {
            if (_target != null)
                _workerMover.MoveToTarget(_target.position);

            yield return new WaitForFixedUpdate();
        }
    }
}
