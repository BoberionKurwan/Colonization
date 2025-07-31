using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private Rock _prefab;
    [SerializeField] private SpawnPoints _spawnPoints;
    [SerializeField] private float _delay = 0.2f;

    private List<Rock> _rocks;
    private Coroutine _spawnCoroutine;

    private void Awake()
    {
        _rocks = new List<Rock>();

        for (int i = 0; i < _spawnPoints.GetEmptyCount(); i++)
        {
            Rock rock = Instantiate(_prefab);
            rock.gameObject.SetActive(false);
            _rocks.Add(rock);
        }
    }

    private void Start()
    {
        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    private Rock GetRock(Transform point)
    {
        Rock rock = _rocks.FirstOrDefault(rock => !rock.isActiveAndEnabled);

        if (rock != null)
        {
            rock = Instantiate(_prefab);
            _rocks.Add(rock);
        }

        rock.gameObject.SetActive(true);
        rock.transform.position = point.position;
        rock.ReturnToPool += ReleaseRock;
        rock.SpawnPoint = point;

        return rock;
    }

    private void ReleaseRock(Rock rock)
    {
        _spawnPoints.ClearBusyPoint(rock.SpawnPoint);
        rock.gameObject.SetActive(false);
        rock.ReturnToPool -= ReleaseRock;

    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            if (_spawnPoints.GetEmptyCount() > 0)
            {
                GetRock(_spawnPoints.GetRandomEmptyPoint());
            }
            yield return delay;
        }
    }
}