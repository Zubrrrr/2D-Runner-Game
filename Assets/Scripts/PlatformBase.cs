using UnityEngine;
using GameManagement;

public class PlatformBase : MonoBehaviour, IPlatform
{
    [SerializeField] protected float scrollSpeed = 9.0f;
    protected bool isMoving = false;

    private void Start()
    {
        GameManager.Instance.RegisterPlatform(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.UnregisterPlatform(this);
    }

    public virtual void StartMoving()
    {
        isMoving = true;
    }

    public virtual void StopMoving()
    {
        isMoving = false;
    }

    protected virtual void Update()
    {
        if (isMoving)
        {
            MovePlatform();
        }
    }

    protected void MovePlatform()
    {
        transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }
}
