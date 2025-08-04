using UnityEngine;
using System.Collections.Generic;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private MainBase _prefab;
    [SerializeField] private MainBase _startBase;
    [SerializeField] private Vector3 _offset;

    private readonly List<MainBase> _bases = new List<MainBase>();

    private void Start()
    {
        _startBase.BuildingBase += Build;
        _bases.Add(_startBase);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void Build(Transform position, Worker worker)
    {
        MainBase newBase = Instantiate(_prefab, position.position + _offset,
            _startBase.transform.rotation);
        newBase.Initialize(worker, _startBase.GetRepository(), this);
        newBase.BuildingBase += Build;
        _bases.Add(newBase);
    }

    private void UnsubscribeEvents()
    {
        foreach (var mainBase in _bases)
        {
            mainBase.BuildingBase -= Build;
        }
    }
}