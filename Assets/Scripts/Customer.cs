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
    private GameObject canvas;
    private Slider patience;
    public List<Chair> chairs;

    void Start()
    {
        canvas = this.transform.GetChild(0).gameObject;
        patience = canvas.transform.GetChild(0).gameObject.GetComponent<Slider>();
        patience.maxValue = 10f;
        machine = GameObject.Find("machine").GetComponent<Machine>();
        currentNode = startNode;
        myChair = chairs.Find(p => p.isOccupied == false);
        myChair.isOccupied = true;
        isServed = false;
    }

    void Update()
    {
        if (manager.idle)
            machine.idle = true;

        if (canvas.activeSelf)
        {
Debug.Log("Patience for customer is " + patience.value);
            patience.value -= Time.deltaTime;
        }

        if (!isServed && patience.value > 0f)
            {
                CreatePath(currentNode, myChair.chairNode);

                if (path.Count == 0 && !hasOrdered)
                {
                    //customer has reached chair, they will now order
                    Order();
                }
            }

        else
        {
            canvas.SetActive(false);
            //increases number served
            if (isServed && myChair.isOccupied)
            {
                manager.numServed++;
                //makes their chair available
                myChair.isOccupied = false;
                //and removes them from machine queue
                machine.serveList.Remove(this);
            }

            CreatePath(myChair.chairNode, startNode);

            //the customer has reached the exit
            if (path.Count == 0)
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
