using UnityEngine;
using System.Collections.Generic;

namespace MainBase
{
    public class WorkerForeman : MonoBehaviour
    {
        public void SendWorkersToCollect(List<Worker> workers, Transform storageTransform, Transform target)
        {
            for (int i = 0; i < workers.Count; i++)
            {
                if (workers[i].StoragePoint == null)
                    workers[i].SetStorage(storageTransform);

                if (workers[i].IsFree)
                {
                    workers[i].SetTarget(target);
                }
            }
        }

        public void SendWorkerToBuild(Worker worker, Transform position)
        {
            worker.SetTarget(position);
        }
    }
}