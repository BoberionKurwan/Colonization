using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;

    private Coroutine _coroutine;

    public event Action<List<Rock>> TerrainScanned;

    private void Start()
    {
        _coroutine = StartCoroutine(ScanRoutine());
    }

    private void Scan()
    {
        List<Rock> rocks = new List<Rock>();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var hitCollider in hitColliders)
        {
            Rock rock = hitCollider.GetComponent<Rock>();

            if (rock.isActiveAndEnabled)
                rocks.Add(rock);
        }

        TerrainScanned?.Invoke(new List<Rock>(rocks));
    }

    private IEnumerator ScanRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while ( enabled)
        {
            Scan();

            yield return delay;
        }
    }
}