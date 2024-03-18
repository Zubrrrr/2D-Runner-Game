using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
    [SerializeField] private GameObject _platformPrefab;
    [SerializeField] private GameObject _firstObject;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _destroyPointX = -30f;
    [SerializeField] private float _respawnPointX = -5f;

    private List<GameObject> _activePlatforms = new List<GameObject>();

    private void Start()
    {
        _activePlatforms.Add(_firstObject);
    }

    private void Update()
    {
        CheckAndSpawnNewPlatform();
        CheckAndRemovePlatforms();
    }

    private void CheckAndSpawnNewPlatform()
    {
        if (_activePlatforms.Count == 0 || _activePlatforms[_activePlatforms.Count - 1].transform.position.x < _respawnPointX)
        {
            GameObject newPlatform = Instantiate(_platformPrefab, _spawnPoint.position, Quaternion.identity);
            _activePlatforms.Add(newPlatform);
        }
    }

    private void CheckAndRemovePlatforms()
    {
        for (int i = _activePlatforms.Count - 1; i >= 0; i--)
        {
            if (_activePlatforms[i].transform.position.x < _destroyPointX)
            {
                GameObject platformToRemove = _activePlatforms[i];
                _activePlatforms.RemoveAt(i);
                Destroy(platformToRemove);
            }
        }
    }
}
