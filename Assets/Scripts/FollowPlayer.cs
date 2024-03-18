using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Vector3 _offset;

    void Update()
    {
        transform.position = _playerTransform.position + _offset;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
