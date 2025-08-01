using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RockPool : MonoBehaviour
{
    private Rock _rockPrefab;
    private List<Rock> _rocks;

    public Rock Get(Transform point)
    {
        Rock rock = _rocks.FirstOrDefault(rock => !rock.isActiveAndEnabled);

        if (rock == null)
            rock = Create();

        rock.gameObject.SetActive(true);
        rock.transform.position = point.position;
        rock.Collected += Release;
        rock.SpawnPoint = point;

        return rock;
    }

    public void Release(Rock rock)
    {
        rock.gameObject.SetActive(false);
        rock.Collected -= Release;
    }

    private Rock Create()
    {
        Rock rock = Instantiate(_rockPrefab);
        _rocks.Add(rock);
        return rock;
    }
}