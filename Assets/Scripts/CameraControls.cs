using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControls : MonoBehaviour
{
    public InputActionAsset InputActions;
    private Camera cam;
    public float sensitivity;
    private InputAction m_zoom;
    private InputAction m_move;
    private Vector2 m_moveAmt;
    private Vector2 m_zoomAmt;

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
    }

    private void Update()
    {
        //read inputs
        m_moveAmt = m_move.ReadValue<Vector2>() / sensitivity;
        m_zoomAmt = m_zoom.ReadValue<Vector2>();

        //move camera
        cam.transform.Translate(m_moveAmt);

        //zoom in (mouse)
        if (m_zoomAmt.y > 0f && cam.orthographicSize >= 6)
            cam.orthographicSize--;
        //zoom out (mouse)
        if (m_zoomAmt.y < 0f && cam.orthographicSize <= 20)
            cam.orthographicSize++;
    }
}