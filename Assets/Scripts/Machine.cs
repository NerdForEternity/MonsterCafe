using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    public InputActionAsset InputActions;
    public CustomerManager manager;
    public UpgradeManager upgrades;
    private float maxCookTime = 3f;
    private float cookTime;
    private bool doneCooking;
    public List<Customer> serveList;
    private Customer currentCustomer;
    private InputAction m_hitScreen;
    private bool isClicked;
    public bool idle;
    private bool idleInProgress;
    private Collider2D collision;
    private GameObject clock;
    private Image radial;

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
        clock = this.transform.GetChild(0).gameObject;
        radial = clock.transform.GetChild(1).GetComponent<Image>();
    }
    private void Awake()
    {
        m_hitScreen = InputSystem.actions.FindAction("Click");

        m_hitScreen.performed += _ =>
        {
            Vector3 clickPos = m_hitScreen.ReadValue<Vector2>();
            clickPos = Camera.main.ScreenToWorldPoint(clickPos);

            if(collision.OverlapPoint(clickPos))
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

            if(clock.activeSelf)
            {
                Debug.Log(cookTime);
                cookTime -= Time.deltaTime;
                radial.fillAmount = (cookTime / (maxCookTime - upgrades.cookSpeedAdd));
            }

            if (idle && !idleInProgress)
            {
                idleInProgress = true;
                Invoke("Serve", 2.0f);
            }

            else if (isClicked && !idle)
            {
                if(!doneCooking)
                    StartCoroutine("Cook");
                if (cookTime <= 0f)
                {
                    Debug.Log("Served customer (active)");
                    currentCustomer.isServed = true;
                    manager.numServed++;
                    //note: generalize when more orders added
                    //ie:
                    //upgrades.money += (order.money);

                    upgrades.totalMoney += 2;
                    cookTime = maxCookTime - upgrades.cookSpeedAdd;
                    isClicked = false;
                }
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
        if(!doneCooking)
            StartCoroutine("Cook");
        if (currentCustomer.patience.value > 0f && cookTime <= 0f)
        {
            Debug.Log("Served customer (idle)");
            currentCustomer.isServed = true;
            manager.numServed++;
            //note: generalize when more orders added
            //ie:
            //upgrades.money += (order.money / 2);
            upgrades.totalMoney++;
            cookTime = maxCookTime - upgrades.cookSpeedAdd;
        }
        idleInProgress = false;
        doneCooking = false; 
    }
    
    IEnumerator Cook()
    {
        Debug.Log("Cook callled");
        doneCooking = true;
        cookTime = maxCookTime - upgrades.cookSpeedAdd;
        clock.SetActive(true);
        yield return new WaitForSeconds(maxCookTime - upgrades.cookSpeedAdd);
        clock.SetActive(false);
    }
}
