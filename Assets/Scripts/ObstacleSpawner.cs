using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Tooltip("Prefab of the object to spawn.")]
    [SerializeField] private GameObject _objPrefab;

    [Tooltip("Array of transforms representing spawn points.")]
    [SerializeField] private Transform[] _spawnPoints;

    [Tooltip("Interval in seconds between each spawn.")]
    [SerializeField] private float _spawnInterval = 2.0f;

    [Tooltip("Internal timer to track spawn intervals.")]
    private float _timer;

    void Start()
    {
        _timer = _spawnInterval;
    }

    void Update()
    {
        _timer -= Time.deltaTime;

        if (_timer <= 0)
        {
            foreach (var point in _spawnPoints)
            {
                SpawnPlatformAtPoint(point);
            }

            _timer = _spawnInterval; 
        }
    }

   private void SpawnPlatformAtPoint(Transform spawnPoint)
    {
        if (PointIsEmpty(spawnPoint) && _objPrefab != null)
        {
            Instantiate(_objPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private bool PointIsEmpty(Transform point)
    {
        return point.childCount == 0;
    }
}
