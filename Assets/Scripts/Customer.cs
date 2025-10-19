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
    private Chair myChair;
    private List<PathNode> path = new List<PathNode>();
    public Machine machine;
    public CustomerManager manager;
    private Animator animator;
    private GameObject canvas;
    private Slider patience;
    public List<Chair> chairs;
    private ParticleSystem particles;

    void Start()
    {
        particles = this.transform.GetChild(0).GetComponent<ParticleSystem>();
        animator = this.transform.GetChild(1).GetChild(1).GetComponent<Animator>();
        canvas = this.transform.GetChild(1).GetChild(0).gameObject;
        patience = canvas.GetComponentInChildren<Slider>(true);
        patience.maxValue = 10f;
        machine = GameObject.Find("machine").GetComponent<Machine>();
        currentNode = startNode;
        myChair = chairs.Find(p => p.isOccupied == false);
        myChair.isOccupied = true;
        isServed = false;
    }

    void Update()
    {
Debug.Log("walking = " + animator.GetBool("Walking"));
Debug.Log("sitting = " + animator.GetBool("Sitting"));
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
Debug.Log("Leaving");
            animator.SetBool("Sitting", false);
            animator.SetBool("Walking", true);
            canvas.SetActive(false);
            //increases number served
            if (isServed && myChair.isOccupied)
            {
Debug.Log("I was served! :)");
                particles.Play();
                manager.numServed++;
            }

            //makes their chair available
            myChair.isOccupied = false;
            //and removes them from machine queue
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
