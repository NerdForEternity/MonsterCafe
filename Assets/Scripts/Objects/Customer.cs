using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private PathNode currentNode;
    public PathNode startNode;
    public bool isServed;
    public bool hasOrdered;
    public bool leaving;
    public Chair myChair;
    private List<PathNode> path = new List<PathNode>();
    //public Machine machine;
    public CustomerManager manager;
    private Animator animator;
    private GameObject canvas;
    public Slider patience;
    private ParticleSystem particles;
    AudioManager audioManager;
    public List<FoodItem> myOrders;
    GameObject orderSprites;
    
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        particles = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        animator = this.transform.GetChild(1).GetChild(1).GetComponent<Animator>();
        canvas = this.transform.GetChild(1).GetChild(0).gameObject;
        patience = canvas.GetComponentInChildren<Slider>(true);
        patience.maxValue = 15f + (UpgradeManager.patienceAdd * 5f);
        patience.value = patience.maxValue;
        currentNode = startNode;
        myChair = manager.chairs.Find(p => p.isOccupied == false);
        myChair.isOccupied = true;

        isServed = false;
    }

    void Update()
    {
        if (canvas.activeSelf)
            patience.value -= Time.deltaTime;

        //runs when customer arrives/waits for order
        if (!isServed && patience.value > 0f)
        {
            CreatePath(currentNode, myChair.chairNode);

            //customer has arrived at chair
            if (path.Count == 0 && !hasOrdered)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Sitting", true);

                Order();
            }
        }

        //runs when the customer leaves
        else
        {
            animator.SetBool("Sitting", false);
            animator.SetBool("Walking", true);
            canvas.SetActive(false);

            if (!leaving)
            {
                myChair.isOccupied = false;
                leaving = true;
            }     

            CreatePath(myChair.chairNode, startNode);

            //the customer has reached the exit
            if (currentNode == startNode && path.Count == 0)
            {
                //subtracts number of customers in scene to spawn more
                manager.numCustomers--;

                Destroy(this.gameObject);
            }
        }
    }

    public void Order()
    {
        hasOrdered = true;
        canvas.SetActive(true);

        myOrders = new List<FoodItem>();
        int numOrders = Random.Range(1, 3);
        orderSprites = canvas.transform.GetChild(1).gameObject;

        int randomOrder = Random.Range(0, 5);
        for (int i = 0; i < numOrders; i++)
        {
            randomOrder = Random.Range(0, 5);
            while (UpgradeManager.orderList[randomOrder].isUnlocked == false)
                randomOrder = Random.Range(0, 5);

            FoodItem nextOrder = UpgradeManager.orderList[randomOrder];
            myOrders.Add(nextOrder);

            if (manager.machines.Any(a => a.itemType == nextOrder))
            {
                Machine myMachine = manager.machines.First(Machine => Machine.itemType == nextOrder);
                myMachine.serveList.Add(this);
            }

            SpriteRenderer currentSprite = orderSprites.transform.GetChild(i).GetComponent<SpriteRenderer>();
            currentSprite.sprite = nextOrder.sprite;
        }

        if (numOrders == 1)
            orderSprites.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void CreatePath(PathNode startNode, PathNode endNode)
    {
        if (path.Count > 0)
        {
            int x = 0;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(path[x].transform.position.x, path[x].transform.position.y), 3 * Time.deltaTime);

            if (Vector2.Distance(transform.position, path[x].transform.position) < 0.1f)
            {
                currentNode = path[x];
                path.RemoveAt(x);
            }
        }

        else
        {
            while (path == null || path.Count == 0)
                path = Pathfinding.instance.GeneratePath(startNode, endNode);
        }
    }

    public void Serve(FoodItem foodItem, bool isIdle)
    {
        if (myOrders.Contains(foodItem))
        {
            for (int i = 0; i < myOrders.Count; i++)
            {
                SpriteRenderer currentSprite = orderSprites.transform.GetChild(i).GetComponent<SpriteRenderer>();
                if (currentSprite.sprite == foodItem.sprite)
                {
                    currentSprite.enabled = false;
                    break;
                }
            }
            myOrders.Remove(foodItem);
        }

        int idleModifier = 1;
        if (isIdle)
            idleModifier++;

        float orderMoney = foodItem.price;
        if (foodItem.numUpgrades > 0)
        {
            for (int i = 0; i < foodItem.numUpgrades; i++)
                orderMoney = orderMoney * 1.2f;
        }
        Mathf.Round(orderMoney);
        UpgradeManager.totalMoney += (int)orderMoney / idleModifier;

        particles.Play();
        audioManager.PlaySFX(audioManager.cookingComplete);

        //all of this customer's items have been served
        if (myOrders.Count == 0)
        {
            isServed = true;
            manager.numServed++;
        }
    }
}
