using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ResourcesRepository : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<MainBase> _mainBases = new List<MainBase>();

    private readonly List<Rock> _emptyResources = new List<Rock>();
    private readonly List<Rock> _busyResources = new List<Rock>();

    private void Start()
    {
        _scanner.FoundResource += AddResource;

        foreach (var building in _mainBases)
        {
            building.RockDelivered += ReturnRock;
        }
    }

    private void OnDestroy()
    {
        _scanner.FoundResource -= AddResource;

        foreach (var building in _mainBases)
        {
            building.RockDelivered += ReturnRock;
        }
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

    private void ReturnRock(Rock rock)
    {
        if (_busyResources.Contains(rock))
        {
            _busyResources.Remove(rock);
            _emptyResources.Add(rock);
        }
        else if (!_emptyResources.Contains(rock))
        {
            _emptyResources.Add(rock);
        }
    }

    private void AddResource(Rock rock)
    {
        if (_emptyResources.Contains(rock) || _busyResources.Contains(rock))
        {
            return;
        }
        else
        {
            _emptyResources.Add(rock);
        }
    }
}