using Unity.Mathematics;
using UnityEngine;

public class WorkerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public void MoveToTarget(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            _speed * Time.deltaTime);
    }
}
