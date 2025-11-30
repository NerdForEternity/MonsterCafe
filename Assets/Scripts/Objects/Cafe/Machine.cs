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
    public Employee myEmployee; // gameobject for employee
    private int employeeMod; // used in price calculation
    //bools
    public bool idle; //checks if idle mode is on
    private bool idleInProgress;  //checks if idle mode is currently handling an order
    private bool doneCooking; //checks if an item is still cooking or not
    //Timer
    private GameObject clock;
    private Image radial;
    private float cookTime;

    void Start()
    {
        collision = this.GetComponent<Collider2D>();
        clock = this.transform.GetChild(0).gameObject;
        radial = clock.transform.GetChild(1).GetComponent<Image>();

        if(myEmployee != null && myEmployee.isUnlocked)
            employeeMod = 2;
        else
            employeeMod = 1;
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
                // can only click on machine manually if in active mode
                // ...OR there isnt an employee on the machine who is unlocked
                if (serveList[0].hasOrdered && !idle && (myEmployee == null || !myEmployee.isUnlocked))
                    IsClicked(false);
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
                radial.fillAmount = cookTime / (itemType.cookTime - (PlayerPrefs.GetInt("CookSpeed", 0)));
            }

            //Customer is served with idle play if idle turned on...
            //OR if this machine has an employee, it is turned on by default
            if ((idle || myEmployee.isUnlocked) && !idleInProgress)
            {
Debug.Log("Invoked idle serve");
                idleInProgress = true;
                Invoke("IdleServe", 2.0f);
            }
        }
    }

    public void IdleServe()
    {
        if ((!idle && !myEmployee.isUnlocked) || doneCooking || currentCustomer.leaving)
        {
Debug.Log("Exited idle serve");
            idleInProgress = false;
            return;
        }

        else
            IsClicked(true);
    }
    
    public void IsClicked(bool isIdle)
    {
        if (!doneCooking && cookTime >= 0f)
            StartCoroutine("Cook", isIdle);
        else if (!doneCooking)
        {
            if(!currentCustomer.leaving)
                currentCustomer.Serve(itemType, isIdle, employeeMod);
            serveList.Remove(currentCustomer);

            //Resets the clock and bools after customr has been served
            cookTime = itemType.cookTime - PlayerPrefs.GetInt("CookSpeed", 0);
            idleInProgress = false;
        }
    }
    
    IEnumerator Cook(bool isIdle)
    {
        doneCooking = true;
        cookTime = itemType.cookTime - PlayerPrefs.GetInt("CookSpeed", 0);
        clock.SetActive(true);
        //turn on employee animator
        if(myEmployee != null)
            myEmployee.animator.SetBool("Cooking", true);
        yield return new WaitForSeconds(itemType.cookTime - PlayerPrefs.GetInt("CookSpeed", 0));
        clock.SetActive(false);
        //turn off employee animator
        if(myEmployee != null)
            myEmployee.animator.SetBool("Cooking", false);
        doneCooking = false;
        IsClicked(isIdle);
    }
}
