using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    public Machine machine;
    public CustomerManager manager;
    //public UpgradeManager upgrades;
    private Animator animator;
    private GameObject canvas;
    public Slider patience;
    public List<Chair> chairs;
    private ParticleSystem particles;
    AudioManager audioManager;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        particles = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        animator = this.transform.GetChild(1).GetChild(1).GetComponent<Animator>();
        canvas = this.transform.GetChild(1).GetChild(0).gameObject;
        patience = canvas.GetComponentInChildren<Slider>(true);
        patience.maxValue = 7f + UpgradeManager.patienceAdd;
        
        //note: fix when there are multiple machines
        machine = GameObject.Find("machine").GetComponent<Machine>();
        machine.manager = manager;
        
        currentNode = startNode;
        myChair = chairs.Find(p => p.isOccupied == false);
        myChair.isOccupied = true;

        isServed = false;
    }

    void Update()
    {
        machine.idle = manager.idle;

        if (canvas.activeSelf)
            patience.value -= Time.deltaTime;


        //runs when customer arrives/waits for order
        if (!isServed && patience.value > 0f)
        {
            CreatePath(currentNode, myChair.chairNode);

            if (!hasOrdered)
            {
                animator.SetBool("Walking", true);
            }

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
                if (isServed)
                {
                    particles.Play();
                    audioManager.PlaySFX(audioManager.cookingComplete);
                }

                myChair.isOccupied = false;
                machine.isClicked = false;
                leaving = true;
            }

            //removes them from machine queue
            machine.serveList.Remove(this);

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
        //note: in later versions customer will randomly choose from unlocked foods but for now will only order coffee
        machine.serveList.Add(this);
        hasOrdered = true;
        canvas.SetActive(true);

        List<FoodItem> myOrders = new List<FoodItem>();
        int numOrders = Random.Range(1, 3);
        GameObject orderSprites = canvas.transform.GetChild(1).gameObject;

Debug.Log("I ordered " + numOrders + " order(s)");
        int randomOrder = Random.Range(0, 5);
        for (int i = 0; i < numOrders; i++)
        {
            randomOrder = Random.Range(0, 5);
            while (UpgradeManager.orderList[randomOrder].isUnlocked == false)
                randomOrder = Random.Range(0, 5);

            FoodItem nextOrder = UpgradeManager.orderList[randomOrder];
            myOrders.Add(nextOrder);

            Debug.Log("I ordered " + myOrders[i].name);
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
}
