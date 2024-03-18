using UnityEngine;
using GameManagement;

public class FlySegmentActivator : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToActivate;
    [SerializeField] private float _scrollSpeed = 9.0f;
    [SerializeField] private float _endPosition = 25f;

    private int _currentObjectIndex = 0;

    private void Start()
    {
        InitializeObjects();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStarted += StartMoving;
    }

    private void Update()
    {
        StartMoving();
    }

    private void InitializeObjects()
    {
        foreach (var obj in _objectsToActivate)
        {
            obj.SetActive(false);
        }
    }

    private void ActivateNextObject()
    {
        if (_currentObjectIndex < _objectsToActivate.Length)
        {
            _objectsToActivate[_currentObjectIndex].SetActive(true);
            _currentObjectIndex++;

            // Если все объекты активированы, начинаем движение
            if (_currentObjectIndex == _objectsToActivate.Length)
            {
                StartMoving();
            }
        }
    }

    private void MoveObject()
    {
        if (_currentObjectIndex == _objectsToActivate.Length)
        {
            transform.position += Vector3.right * _scrollSpeed * Time.deltaTime;

            if (transform.position.x > _endPosition)
            {
                Destroy(gameObject);
            }
        }
    }

    private void StartMoving()
    {
        ActivateNextObject();
        MoveObject();
    }
}
