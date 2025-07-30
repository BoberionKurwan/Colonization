using UnityEngine;
using TMPro;

public class RocksCollectedDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Storage _storage;

    private void Start()
    {
        UpdateUI();

        _storage.CollectedRock += UpdateUI;
    }

    private void OnDestroy()
    {
        _storage.CollectedRock -= UpdateUI;
    }

    private void UpdateUI()
    {
        _text.text = $"Collected Rocks: {_storage.CollectedRockCount}";

        if (_storage.IsStorageFull)
        {
            _text.text += "\n Storage is Full!" ;
        }
    }
}
