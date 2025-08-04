using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action<Flag> BeTaken;

    public void InvokeTaken()
    {
        BeTaken?.Invoke(this);
    }
}