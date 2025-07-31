using UnityEngine;
using System.Collections.Generic;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _layerMask;

    private List<Rock> _rocks = new List<Rock>();

    public List<Rock> GetRocks()
    {
        Scan();

        return _rocks;
    }

    private void Scan()
    {
        _rocks.Clear();

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _scanRadius, _layerMask);

        foreach (var hitCollider in hitColliders)
        {
            Rock rock = hitCollider.GetComponent<Rock>();

            _rocks.Add(rock);
        }
    }
}
