using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;

    HashSet<Rock> _resources = new HashSet<Rock>();

    private Coroutine _coroutine;

    public event Action<HashSet<Rock>> Scanned;

    private void Start()
    {
        _coroutine = StartCoroutine(ScanRoutine());
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var hitCollider in hitColliders)
        {
            Rock rock = hitCollider.GetComponent<Rock>();

            if (rock.isActiveAndEnabled)
                _resources.Add(rock);
        }

        Scanned?.Invoke(_resources);
    }

    private IEnumerator ScanRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            Scan();

            yield return delay;
        }
    }
}
