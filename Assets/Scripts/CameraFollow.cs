using UnityEngine;
using GameManagement;

public class CameraFollow : MonoBehaviour
{
    [SerializeField, Range(1f, 10f)]
    [Tooltip("Maximum size of the camera's orthographic view when zoomed out.")]
    private float _zoomOutSize = 10f;

    [SerializeField, Range(1f, 10f)]
    [Tooltip("Minimum size of the camera's orthographic view when zoomed in.")]
    private float _zoomInSize = 5f;

    [SerializeField, Range(1f, 20f)]
    [Tooltip("Speed at which the camera zooms in and out.")]
    private float _zoomSpeed = 1f;

    [SerializeField]
    [Tooltip("Height at which the camera starts moving up.")]
    private float _heightThreshold = 5f;

    [SerializeField, Range(0.1f, 10f)]
    [Tooltip("Speed at which the camera follows the player upwards.")]
    private float _followUpSpeed = 2f;

    [SerializeField, Range(0.1f, 10f)]
    [Tooltip("Speed at which the camera follows the player downwards.")]
    private float _followDownSpeed = 3f; // Быстрее чем _followUpSpeed

    private Camera _cam;
    private Transform _playerTransform;
    private Vector3 _startPosition;
    private bool _isZoomingOut = false;

    private void OnDisable()
    {
        GameManager.Instance.OnGameStarted -= AdjustZoom;
        GameManager.Instance.OnGameStarted -= FollowPlayer;
        GameManager.Instance.OnPlayerJumped -= () => SetZoomingOut(true);
        GameManager.Instance.OnPlayerLanded -= () => SetZoomingOut(false);
    }

    private void Start()
    {
        _cam = GetComponent<Camera>();
        InitializeCamera();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _startPosition = transform.position;

        GameManager.Instance.OnGameStarted += AdjustZoom;
        GameManager.Instance.OnGameStarted += FollowPlayer;
        GameManager.Instance.OnPlayerJumped += () => SetZoomingOut(true);
        GameManager.Instance.OnPlayerLanded += () => SetZoomingOut(false);
    }

    private void LateUpdate()
    {
        AdjustZoom();
        FollowPlayer();
    }

    private void InitializeCamera()
    {
        _cam.orthographicSize = _zoomInSize;
    }

    private void AdjustZoom()
    {
        float targetSize = _isZoomingOut ? _zoomOutSize : _zoomInSize;
        _cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, targetSize, Time.deltaTime * _zoomSpeed);
    }

    private void FollowPlayer()
    {
        float followSpeed = _playerTransform.position.y > transform.position.y ? _followUpSpeed : _followDownSpeed;
        Vector3 targetPosition = new Vector3(_startPosition.x, _playerTransform.position.y, _startPosition.z);

        if (targetPosition.y < _startPosition.y)
        {
            targetPosition.y = _startPosition.y;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    private void SetZoomingOut(bool isZoomingOut)
    {
        _isZoomingOut = isZoomingOut;
    }
}
