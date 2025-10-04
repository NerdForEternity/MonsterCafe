using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public List<Chair> chairs;

    void Start()
    {
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

        if (!isServed)
            {
                CreatePath(currentNode, myChair.chairNode);

                if (path.Count == 0 && !hasOrdered)
                {
                    //customer has reached chair, they will now order
                    //note: in later versions customer will randomly choose from unlocked foods but for now will only order coffee
                    machine.serveList.Add(this);
                    //this prevents the if statement from running again
                    hasOrdered = true;
                }
            }
            else
            {
                CreatePath(myChair.chairNode, startNode);
                if (isServed && path.Count == 0)
                {
                    //makes their chair available
                    myChair.isOccupied = false;
                    //subtracts number of customers in scene to spawn more
                    manager.numCustomers--;
                    //increases number served for spawn timer
                    manager.numServed++;
                    //and removes them from machine queue
                    //machine.serveList.Remove(this);
                    Destroy(this.gameObject);
                }
            }
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
