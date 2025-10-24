using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    public InputActionAsset InputActions;
    public CustomerManager manager;
    //public UpgradeManager upgrades;
    private float maxCookTime = 3f;
    private float cookTime;
    private bool doneCooking;
    public List<Customer> serveList;
    private Customer currentCustomer;
    private InputAction m_hitScreen;
    public bool isClicked;
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

            if(clock.activeSelf)
            {
                cookTime -= Time.deltaTime;
                radial.fillAmount = (cookTime / (maxCookTime - UpgradeManager.cookSpeedAdd));
            }

            if (idle && !idleInProgress)
            {
                idleInProgress = true;
                Invoke("Serve", 2.0f);
            }

            //Customer served with active play
            else if (isClicked && !idle)
            {
                if (!doneCooking)
                    StartCoroutine("Cook");
                if (cookTime <= 0f)
                {
                    currentCustomer.isServed = true;
                    manager.numServed++;
                    //note: generalize when more orders added
                    //ie:
                    //UpgradeManager.money += (order.money);

                    float orderMoney = 2f;
                    if (UpgradeManager.priceAdd > 0)
                    {
                        for (int i = 0; i < UpgradeManager.priceAdd; i++)
                            orderMoney = orderMoney * 1.2f;
                    }
                    Mathf.Round(orderMoney);
                    UpgradeManager.totalMoney += (int)orderMoney;

                    cookTime = maxCookTime - UpgradeManager.cookSpeedAdd;
                    isClicked = false;
                    doneCooking = false;
                }
            }
        }
    }

    public void Serve()
    {
        if (!idle || currentCustomer.isServed)
        {
            idleInProgress = false;
            //doneCooking = false; 
            return;
        }
        //if(!doneCooking)
            //StartCoroutine("Cook");
        //if (currentCustomer.patience.value > 0f && cookTime <= 0f)
        if(currentCustomer.patience.value > 0f)
        {
            currentCustomer.isServed = true;
            manager.numServed++;
            //note: generalize when more orders added
            //ie:
            //UpgradeManager.money += (order.money / 2);
            
            float orderMoney = 1f;
            if (UpgradeManager.priceAdd > 0)
            {
                for (int i = 0; i < UpgradeManager.priceAdd; i++)
                orderMoney = orderMoney * 1.2f;
            }
            Mathf.Round(orderMoney);
            UpgradeManager.totalMoney += (int)orderMoney;
            //cookTime = maxCookTime - UpgradeManager.cookSpeedAdd;
        }
        idleInProgress = false;
        //doneCooking = false; 
    }
    
    IEnumerator Cook()
    {
        doneCooking = true;
        cookTime = maxCookTime - UpgradeManager.cookSpeedAdd;
        clock.SetActive(true);
        yield return new WaitForSeconds(maxCookTime - UpgradeManager.cookSpeedAdd);
        clock.SetActive(false);
        doneCooking = false;
    }
}
