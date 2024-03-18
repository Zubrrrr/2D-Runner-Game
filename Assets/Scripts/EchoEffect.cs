using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    [SerializeField, Range(0f, 5f)] private float _timeBtwSpawns;
    [SerializeField, Range(0f, 5f)] private float _startTimeBTWSpawns;
    [SerializeField, Range(0f, 5f)] private float _echoSpeed;
    [SerializeField, Range(0f, 5f)] private float _echoLifeTime;

    [SerializeField] private GameObject[] _echoPrefabs;

    private List<GameObject> _activeEchos = new List<GameObject>();
    void Update()
    {
        TrySpawnEcho();
    }

    private void TrySpawnEcho()
    {
        if (_timeBtwSpawns <= 0)
        {
            int randomIndex = Random.Range(0, _echoPrefabs.Length);
            GameObject echoPrefab = _echoPrefabs[randomIndex];
            GameObject newEcho = Instantiate(echoPrefab, transform.position, Quaternion.identity);
            _activeEchos.Add(newEcho);

            _timeBtwSpawns = _startTimeBTWSpawns;
            StartCoroutine(MoveEcho(newEcho));
        }
        else
        {
            _timeBtwSpawns -= Time.deltaTime;
        }
    }

    IEnumerator MoveEcho(GameObject echo)
    {
        float elapsedTime = 0;

        while (elapsedTime < _echoLifeTime)
        {
            echo.transform.Translate(-_echoSpeed * Time.deltaTime, 0, 0);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        _activeEchos.Remove(echo);
        Destroy(echo);
    }

    private void OnDisable()
    {
        foreach (var echo in _activeEchos)
        {
            Destroy(echo);
        }
        _activeEchos.Clear();
    }
}
