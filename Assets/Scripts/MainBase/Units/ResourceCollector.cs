using System;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    [SerializeField] private Transform _carryingPosition;

    private Transform _target;
    private Rock _currentRock;

    public event Action Collected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rock rock))
        {
            if (rock.transform == _target)
            {
                PickUpRock(rock);
                Collected?.Invoke();
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public Rock GiveRock()
    {
        if (_currentRock == null)
            return null;

        Rock rockToGive = _currentRock;
        rockToGive.transform.SetParent(null);
        _currentRock = null; 
        return rockToGive;
    }

    private void PickUpRock(Rock rock)
    {
        _currentRock = rock;
        rock.transform.position = _carryingPosition.position;
        rock.transform.SetParent(_carryingPosition);
    }
}
