using System;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    [SerializeField] private Transform _carryingPosition;

    private Rock _rock;

    public event Action Collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rock rock))
        {
            if (rock == _rock)
            {
                PickUpRock(rock);
                Collected?.Invoke();
            }
        }
    }

    public void SetTarget(Rock rock)
    {
        _rock = rock;
    }

    public Rock GiveRock()
    {
        if (_rock == null)
            return null;

        Rock rock = _rock;
        _rock.transform.SetParent(null);
        _rock = null;
        return rock;
    }

    private void PickUpRock(Rock rock)
    {
        rock.transform.position = _carryingPosition.position;
        rock.transform.SetParent(_carryingPosition);
    }
}
