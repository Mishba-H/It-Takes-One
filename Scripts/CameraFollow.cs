using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    #region VARIABLES

    [SerializeField] private Transform follow;
    [SerializeField] private float dampTime;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;

    #endregion

    void LateUpdate()
    {
        if (follow != null)
        {
            targetPosition = follow.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, dampTime);
        }
    }
}
