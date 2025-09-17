using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraControls : MonoBehaviour
{
    public Camera cam;
    public InputActionAsset InputActions;
    public float sensitivity;
    private InputAction m_zoom;
    private InputAction m_move;
    private Vector2 m_moveAmt;
    private Vector2 m_zoomAmt;

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

        cam.transform.Translate(m_moveAmt / sensitivity);

        //zoom in
        if (m_zoomAmt.y > 0f && cam.orthographicSize >= 6)
            cam.orthographicSize--;
        //zoom out
        if (m_zoomAmt.y < 0f && cam.orthographicSize <= 20)
            cam.orthographicSize++;
    }
}
