using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Machine : MonoBehaviour
{
    public InputActionAsset InputActions;
    public CustomerManager manager;
    public UpgradeManager upgrades;
    public List<Customer> serveList;
    private Customer currentCustomer;
    private InputAction m_hitScreen;
    private bool isClicked;
    public bool idle;
    private bool idleInProgress;
    private Collider2D collision;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    void Start()
    {
        collision = this.GetComponent<Collider2D>();
    }
    private void Awake()
    {
        m_hitScreen = InputSystem.actions.FindAction("Click");

        m_hitScreen.performed += _ =>
        {
            Vector3 clickPos = m_hitScreen.ReadValue<Vector2>();
            clickPos = Camera.main.ScreenToWorldPoint(clickPos);

            if (collision.OverlapPoint(clickPos))
            {
                if (serveList[0] != null)
                {
                    if (serveList[0].hasOrdered)
                        isClicked = true;
                }
            }
        };
    }

    void Update()
    {
        if (serveList.Count > 0)
        {
            currentCustomer = serveList[0];
            Debug.Log("Current customer is at " + currentCustomer.myChair.chairNode);
            if (idle && !idleInProgress)
            {
                idleInProgress = true;
                Invoke("Serve", 2.0f);
            }

            else if (isClicked && !idle)
            {
                Debug.Log("Served customer (active)");
                currentCustomer.isServed = true;
                manager.numServed++;
                //note: generalize when more orders added
                //ie:
                //upgrades.money += (order.money);
                upgrades.totalMoney += 2;
                isClicked = false;
            }
        }
    }

    public void Serve()
    {
        Debug.Log("Serve called");
        if (!idle || currentCustomer.isServed)
        {
            idleInProgress = false;
            return;
        }

        Debug.Log("Served customer (idle)");
        currentCustomer.isServed = true;
        manager.numServed++;
        //note: generalize when more orders added
        //ie:
        //upgrades.money += (order.money / 2);
        upgrades.totalMoney++;
        idleInProgress = false;
    }
}
