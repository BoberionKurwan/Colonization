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
        StartCoroutine(SpawnRoutine());
    }

    private Rock Get(Transform point)
    {
        Rock rock = _rocks.FirstOrDefault(rock => !rock.isActiveAndEnabled);

        if (rock == null)
        {
            rock = Instantiate(_prefab);
            _rocks.Add(rock);
        }

        rock.gameObject.SetActive(true);
        rock.transform.position = point.position;
        rock.Collected += ReleaseRock;
        rock.SpawnPoint = point;

        return rock;
    }

    private void ReleaseRock(Rock rock)
    {
        _spawnPoints.ReleaseSpawnPoint(rock.SpawnPoint);
        rock.gameObject.SetActive(false);
        rock.Collected -= ReleaseRock;
    }

    private IEnumerator SpawnRoutine()
    {
        WaitForSeconds delay = new WaitForSeconds(_delay);

        while (enabled)
        {
            if (_spawnPoints.TryGetRandomEmptyPoint(out Transform point))
            {
                Get(point);
            }

            yield return delay;
        }
    }
}