using UnityEngine;
using  UnityEngine.InputSystem;

public class Dragging : MonoBehaviour
{
    private Camera wireCam;
    private InputAction touchPositionAction;
    private InputAction touchPressAction;
    private InputAction dragAction;
    private GameObject clickedObject;
    private int wireCount = 4;

    void Awake()
    {
        wireCam = GetComponentInChildren<Camera>();
        touchPressAction = GetComponent<PlayerInput>().actions.FindAction("TouchPress");
        touchPositionAction = GetComponent<PlayerInput>().actions.FindAction("TouchPosition");
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
        touchPositionAction.performed += TouchMove;
        touchPressAction.canceled += TouchEnded;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
        touchPositionAction.performed -= TouchMove;
        touchPressAction.canceled -= TouchEnded;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        Vector3 position = wireCam.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = 0;
        Debug.Log(position);
        
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
        if(hit.collider != null)
        {
            clickedObject = hit.collider.gameObject;
        }
    }

    private void TouchMove(InputAction.CallbackContext context)
    {
        Vector3 position = wireCam.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z = 0;

        if(clickedObject != null)
        {
            clickedObject.GetComponent<Wire>().DragWire(position);
        }
    }

    private void TouchEnded(InputAction.CallbackContext context)
    {
        if(clickedObject != null)
        {
            clickedObject.GetComponent<Wire>().EndDrag();

            if(clickedObject.GetComponent<Wire>().IsWireHooked())
            {
                wireCount--;
                if(wireCount == 0)
                {
                    GameManager.Instance.WireObjectsActivation(false);
                }
            }
        }

        clickedObject = null;
    }
}