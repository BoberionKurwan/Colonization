using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourcesRepository : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;

    private HashSet<Rock> _emptyResources = new HashSet<Rock>();
    private HashSet<Rock> _busyResources = new HashSet<Rock>();

    private void Start()
    {
        _scanner.Scanned += SetResources;
    }

    private void OnDestroy()
    {
        _scanner.Scanned -= SetResources;
    }

    public bool TryGetRock(out Rock rock)
    {
        if (_emptyResources.Count > 0)
        {
            rock = _emptyResources.First();
            _busyResources.Add(rock);
            _emptyResources.Remove(rock);
            return true;
        }

        rock = null;
        return false;
    }

    private void SetResources(HashSet<Rock> resources)
    {
        _emptyResources = resources;
    }
}