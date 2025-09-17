using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    public Camera cam;
    public InputActionAsset InputActions;
    private InputAction m_zoom;
    private InputAction m_move;

    private Vector2 m_moveAmt;
    private Vector2 m_zoomAmt;

    public void Start()
    {
        cam.orthographicSize = 6f;
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

    }

    private void Update()
    {
        m_moveAmt = m_move.ReadValue<Vector2>();
        m_zoomAmt = m_zoom.ReadValue<Vector2>();

        //m_zoomAmt.y = Mathf.Clamp(m_zoomAmt.y, 6f, 20f);

        cam.orthographicSize = m_zoomAmt.y;

Debug.Log("ZoomAmt = " + m_zoomAmt.y + ", Cam Size = " + cam.orthographicSize);
    }
}
