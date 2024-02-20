using UnityEngine;

public class SpaceShooterCamera : MonoBehaviour
{
    //private fields
    private Vector3 followOffset = new Vector3(0, 3.5f, -10);
    private float smoothTime = 0.125f;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private Transform target;
    private float shakeOffset = 0.05f;
    private bool isShaking;
    private Vector3 originalPosition;

    void FixedUpdate()
    {
        Vector3 targetPosition = target.position + followOffset;
        targetPosition.x = 0;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        originalPosition = transform.localPosition;
        if(isShaking)
        {
            ShakeCamera();
        }
    }

    private void ShakeCamera()
    {
        float x = Random.Range((originalPosition.x - shakeOffset), (originalPosition.x + shakeOffset));
        float y = Random.Range((originalPosition.y - shakeOffset), (originalPosition.y + shakeOffset));

        transform.localPosition = new Vector3(x, y, originalPosition.z);
    }

    public void ActivateShake(bool activation)
    {
        isShaking = activation;
    }
}