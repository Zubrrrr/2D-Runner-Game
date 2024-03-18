using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    [SerializeField] private float _endPosition = -10f;

    private void FixedUpdate()
    {
        if (transform.position.x < _endPosition)
        {
            Destroy(gameObject);
        }
    }
}
