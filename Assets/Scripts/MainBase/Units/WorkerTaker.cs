using Unity.VisualScripting;
using UnityEngine;

public class WorkerTaker : MonoBehaviour
{
    [SerializeField] private Transform _carryingPosition;

    public void PickUpRock(Rock rock)
    {
        rock.transform.position = _carryingPosition.position;
        rock.transform.SetParent(_carryingPosition);
    }
}
