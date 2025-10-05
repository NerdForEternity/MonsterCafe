using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Machine : MonoBehaviour
{
    public InputActionAsset InputActions;
    public GameObject serveIndicator;
    public List<Customer> serveList;
    private InputAction m_hitScreen;
    private bool isClicked;
    public bool idle;
    private bool idleInProgress;

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
        m_hitScreen = InputSystem.actions.FindAction("Click");

        m_hitScreen.performed += _ =>
        {
            Vector3 clickPos = m_hitScreen.ReadValue<Vector2>();
            clickPos = Camera.main.ScreenToWorldPoint(clickPos);
//Debug.Log("Click position: " + clickPos.x + ", " + clickPos.y);

            if (Vector3.Distance(clickPos, this.transform.position) <= 3)
            {
                //Debug.Log("Hit machine!");
                if (serveList[0] != null)
                {
                    if (serveList[0].hasOrdered)
                        isClicked = true;
                }
            }

        };
    }

    void Start()
    {
        serveIndicator.SetActive(false);
    }

    void Update()
    {
        if (serveList.Count > 0)
        {
            serveIndicator.SetActive(true);

            if (idle && !idleInProgress)
            {
Debug.Log("Invoked idle, waiting...");
                idleInProgress = true;
                Invoke("Serve", 2.0f);
            }

            else if (isClicked)
            {
Debug.Log("Served customer (active)");
                serveList[0].isServed = true;
                //serveList.RemoveAt(0);
                isClicked = false;
            }
        }
        else
            serveIndicator.SetActive(false);
    }

    public void Serve()
    {
        if (!idle)
            return;

Debug.Log("Served customer (idle)");
        serveList[0].isServed = true;
        serveList.RemoveAt(0);
        idleInProgress = false;
    }
}
