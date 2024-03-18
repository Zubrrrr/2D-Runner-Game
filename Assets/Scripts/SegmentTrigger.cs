using UnityEngine;
using GameManagement;

public class SegmentTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.ActivateNextSegment();
        }
    }
}
