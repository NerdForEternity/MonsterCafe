using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    //input
    public InputActionAsset InputActions;
    private InputAction m_hitScreen;
    private Collider2D collision;
    //managers and objects
    public CustomerManager manager; 
    public FoodItem itemType;
    public List<Customer> serveList;
    private Customer currentCustomer;
    //bools
    public bool idle; //checks if idle mode is on
    private bool idleInProgress;  //checks if idle mode is currently handling an order
    private bool doneCooking; //checks if an item is still cooking or not
    //Timer
    private GameObject clock;
    private Image radial;
    private float cookTime;

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
                        IsClicked(false);
                }
            }
        };
    }

    void Update()
    {
        idle = manager.idle;
        if (serveList.Count > 0)
        {
            currentCustomer = serveList[0];

            if (currentCustomer.leaving)
                serveList.Remove(currentCustomer);

            //Item is currently being prepared...
            //so clock is turned on
            if (clock.activeSelf)
            {
                cookTime -= Time.deltaTime;
                radial.fillAmount = (cookTime / (itemType.cookTime - UpgradeManager.cookSpeedAdd));
            }

            //Customer is served with idle play
            if (idle && !idleInProgress)
            {
                idleInProgress = true;
                Invoke("IdleServe", 2.0f);
            }
        }
    }

    public void IdleServe()
    {
        if (!idle || currentCustomer.leaving)
        {
            idleInProgress = false;
            return;
        }

        else
            IsClicked(true);
    }
    
    public void IsClicked(bool isIdle)
    {
        if (!doneCooking && currentCustomer.myOrders.Contains(itemType))
            StartCoroutine("Cook", isIdle);
        else if (cookTime <= 0f)
        {
            currentCustomer.Serve(itemType, isIdle);
            serveList.Remove(currentCustomer);

            //Resets the clock and bools after customr has been served
            cookTime = itemType.cookTime - UpgradeManager.cookSpeedAdd;
            idleInProgress = false;
            doneCooking = false;
        }
    }
    
    IEnumerator Cook(bool isIdle)
    {
        doneCooking = true;
        cookTime = itemType.cookTime - UpgradeManager.cookSpeedAdd;
        clock.SetActive(true);
        yield return new WaitForSeconds(itemType.cookTime - UpgradeManager.cookSpeedAdd);
        clock.SetActive(false);
        IsClicked(isIdle);
    }
}
