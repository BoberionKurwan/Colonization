using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _layerMask;

    public event Action<Rock> FoundResource;

    private void Start()
    {
        StartCoroutine(ScanRoutine());
    }

    private void Scan()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius, _layerMask);

        foreach (var hitCollider in hitColliders)
        {
            Rock rock = hitCollider.GetComponent<Rock>();

            if (rock.isActiveAndEnabled)
                FoundResource?.Invoke(rock);
        }
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
