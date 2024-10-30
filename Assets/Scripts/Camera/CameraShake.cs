using UnityEngine;

public class CameraShake : MonoBehaviour
{
    //private fields
    private const float offset = 0.05f;
    private bool isShaking;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
    }

    void Update()
    {
        if(isShaking)
        {
            ShakeCamera();
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    private void ShakeCamera()
    {
        float x = Random.Range(originalPosition.x - offset, originalPosition.x + offset);
        float y = Random.Range(originalPosition.y - offset, originalPosition.y + offset);

        transform.localPosition = new Vector3(x, y, originalPosition.z);
    }

    public void ActivateShake(bool activation)
    {
        isShaking = activation;
    }
}