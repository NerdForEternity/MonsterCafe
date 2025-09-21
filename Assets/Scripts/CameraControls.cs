using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    public InputActionAsset InputActions;
    private Camera cam;
    public float sensitivity;
    private InputAction m_zoom;
    private InputAction m_move;
    private InputAction m_touch0;
    private InputAction m_touch1;
    private InputAction m_isTouch0;
    private InputAction m_isTouch1;
    private Vector2 m_moveAmt;
    private Vector2 m_zoomAmt;
    private int touchCount;
    private float prevDistance = 0;
    private float distance;
    void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        m_zoom = InputSystem.actions.FindAction("Zoom");
        m_move = InputSystem.actions.FindAction("MoveCamera");
        m_touch0 = InputSystem.actions.FindAction("Touch0Position");
        m_touch1 = InputSystem.actions.FindAction("Touch1Position");
        m_isTouch0 = InputSystem.actions.FindAction("Touch0Start");
        m_isTouch1 = InputSystem.actions.FindAction("Touch1Start");

        //detects how many fingers are touching the screen
        m_isTouch0.started += _ => touchCount++;
        m_isTouch1.started += _ => touchCount++;

        m_isTouch0.canceled += _ => touchCount--;
        m_isTouch1.canceled += _ => touchCount--;

        m_touch1.performed += _ =>
        {
            //will only zoom if two fingers are on screen
            if (touchCount == 2)
            {
                distance = Vector2.Distance(m_touch0.ReadValue<Vector2>(), m_touch1.ReadValue<Vector2>());
                if (prevDistance == 0)
                    prevDistance = distance;
                float difference = distance - prevDistance;
                prevDistance = distance;

                if (difference > 0f && cam.orthographicSize >= 6)
                    cam.orthographicSize--;
                else if (difference < 0f && cam.orthographicSize <= 20)
                    cam.orthographicSize++;
            }
        };
    }

    private void Update()
    {
        //read inputs
        m_moveAmt = m_move.ReadValue<Vector2>() / sensitivity;
        m_zoomAmt = m_zoom.ReadValue<Vector2>();

        //move camera if zoom is not in progress
        if (touchCount < 2)
        cam.transform.Translate(m_moveAmt * -1);

        //zoom in (mouse)
        if (m_zoomAmt.y > 0f && cam.orthographicSize >= 6)
            cam.orthographicSize--;
        //zoom out (mouse)
        if (m_zoomAmt.y < 0f && cam.orthographicSize <= 20)
            cam.orthographicSize++;
    }
}