using UnityEngine;

public class Wire : MonoBehaviour
{
    public string color;
    private Vector3 startPoint;
    private Vector3 startPosition;
    private SpriteRenderer wireEnd;
    private bool wireHooked;
    private Vector3 hookedPosition; 

    void Start()
    {
        startPoint = transform.GetChild(0).position;
        wireEnd = transform.GetChild(0).GetComponent<SpriteRenderer>();
        startPosition = transform.position;
    }

    public void DragWire(Vector3 position)
    {
        if(!wireHooked)
        {
            if(position.x > (startPosition.x + 0.22f) && position.y < 3.2f && position.y > -3.2f)
            {
                UpdateWirePosition(position);
            }
        }
    }

    public void EndDrag()
    {
        if(!wireHooked)
        {
            UpdateWirePosition(startPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == color)
        {
            AudioManager.Instance.Play("Accomplished");

            wireHooked = true;

            hookedPosition = other.transform.position;
            UpdateWirePosition(hookedPosition);
        }
    }

    private void UpdateWirePosition(Vector3 newPosition)
    {
        transform.position = newPosition;

        //Update direction
        Vector3 direction = newPosition - startPoint;
        transform.right = direction;

        //Update scale
        float distance = Vector2.Distance(newPosition, startPoint);
        wireEnd.size = new Vector2(distance, wireEnd.size.y);
    }

    public bool IsWireHooked()
    {
        return wireHooked;
    }
}