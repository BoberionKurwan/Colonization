using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public event Action SendWorkersToCollect;
    public event Action ScanForResourses;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            SendWorkersToCollect?.Invoke();
        if(Input.GetKeyDown(KeyCode.E))
            ScanForResourses?.Invoke();
    }
}
