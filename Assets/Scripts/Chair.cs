using UnityEngine;

public class Chair : MonoBehaviour
{
    public PathNode chairNode;
    public bool isOccupied;

    private Vector2 chairPos;
    private PathNode closestNode;

    //note: add to a gamemanager for all moveable objects to reference
    public PathNode GetClosestNode()
    {
        //note: write code to update position if moved by player
        chairPos = this.transform.position;
        float minDistance = Mathf.Infinity;
        foreach (PathNode n in FindObjectsByType<PathNode>(FindObjectsSortMode.None))
        {
            Vector2 currentNodePos = n.transform.position;

            float dist = Vector2.Distance(n.transform.position, chairPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestNode = n;
            }
        }

        return closestNode;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.chairNode = GetClosestNode();
Debug.Log("The node of " + this.name + " is " + chairNode.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
