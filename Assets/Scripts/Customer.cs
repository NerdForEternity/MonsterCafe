using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    private PathNode currentNode;
    public PathNode startNode;
    public GameObject door;
    public Vector2 doorPos;
    private Chair myChair;
    private List<PathNode> path = new List<PathNode>();

    public List<Chair> chairs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentNode = startNode;
        myChair = chairs.Find(p => p.isOccupied == false);
        myChair.isOccupied = true;
    }

    // Update is called once per frame
    void Update()
    {
        CreatePath();
    }

    public void CreatePath()
    {
        if (path.Count > 0)
        {
Debug.Log("Path Count: " + path.Count);
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
Debug.Log("Creating path");
            while (path == null || path.Count == 0)
                path = Pathfinding.instance.GeneratePath(currentNode, myChair.chairNode);
        }
    }
}
