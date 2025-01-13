using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    [SerializeField]
     private float smoothTime = 0.24f;
    [SerializeField]
     private Transform target;

    private Vector3 offset = new Vector3(0, 0, -10); // 카메라 위치
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
