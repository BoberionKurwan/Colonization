using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[RequireComponent(typeof(WorkerMover), typeof(ResourceCollector))]
public class Worker : MonoBehaviour
{
    private WorkerMover _workerMover;
    private ResourceCollector _resourceCollector;

    private Transform _target;
    private Transform _storagePoint;

    private Coroutine _movingCoroutine;

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

    public void SetTarget(Rock rock)
    {
        IsFree = false;
        _target = rock.transform;
        _resourceCollector.SetTarget(rock);
        _movingCoroutine = StartCoroutine(MoveRoutine());
    }

    public void SetStorage(Transform storagePoint)
    {
        _storagePoint = storagePoint;
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
        _target = _storagePoint;
        IsCarryingResource = true;
    }

    private IEnumerator MoveRoutine()
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

        while (_target.position != null)
        {
            _workerMover.MoveToTarget(_target.position);
            yield return new WaitForFixedUpdate();
        }
    }
}
