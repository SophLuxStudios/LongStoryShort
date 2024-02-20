using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Vector3 offset = new Vector3(0, 3.5f, -10);
    private float smoothTime = 0.125f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        targetPosition.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}