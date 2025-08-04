using System;
using UnityEngine;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;

    public event Action<Worker> WorkerCame;

    public Flag CurrentFlag { get; private set; }
    public bool IsFlagActive { get; private set; } = false;
    public Worker SentWorker { get; private set; }

    public void SetPosition(Vector3 position)
    {
        if (CurrentFlag == null)
        {
            CurrentFlag = Instantiate(_flagPrefab, transform);
            CurrentFlag.gameObject.SetActive(false);
            CurrentFlag.BeTaken += RemoveFlag;
        }

        CurrentFlag.transform.position = position;

        if (IsFlagActive == false)
        {
            CurrentFlag.gameObject.SetActive(true);
            IsFlagActive = true;
        }
    }

    public void SetWorker(Worker worker)
    {
        SentWorker = worker;
        IsFlagActive = false;
    }

    public Transform GetFlagPosition()
    {
        return CurrentFlag.transform;
    }

    private void RemoveFlag(Flag flag)
    {
        WorkerCame?.Invoke(SentWorker);
        flag.gameObject.SetActive(false);
        IsFlagActive = false;
    }
}
