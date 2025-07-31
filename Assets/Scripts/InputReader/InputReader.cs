using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public event Action SpaceClicked;
    public event Action EClicked;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            SpaceClicked?.Invoke();
        if(Input.GetKeyDown(KeyCode.E))
            EClicked?.Invoke();
    }
}
