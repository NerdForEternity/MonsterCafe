using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    private void Awake()
    {
        instance = this;
    }

    public List<PathNode> GeneratePath(PathNode start, PathNode end)
    {
        List<PathNode> openSet = new List<PathNode>();

        foreach (PathNode n in FindObjectsByType<PathNode>(FindObjectsSortMode.None))
        {
            n.gScore = float.MaxValue;
        }

        start.gScore = 0;
        start.hScore = Vector2.Distance(start.transform.position, end.transform.position);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            int lowestF = default;

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FScore() < openSet[lowestF].FScore())
                    lowestF = i;
            }
            PathNode currentNode = openSet[lowestF];
            openSet.Remove(currentNode);

            if (currentNode == end)
            {
                List<PathNode> path = new List<PathNode>();
                path.Insert(0, end);
                while (currentNode != start)
                {
                    currentNode = currentNode.prevNode;
                    path.Add(currentNode);
                }

                path.Reverse();
                return path;
            }

            foreach (PathNode connectedNode in currentNode.connections)
            {
                float heldGScore = currentNode.gScore + Vector2.Distance(currentNode.transform.position, connectedNode.transform.position);
                if (heldGScore < connectedNode.gScore)
                {
                    connectedNode.prevNode = currentNode;
                    connectedNode.gScore = heldGScore;
                    connectedNode.hScore = Vector2.Distance(connectedNode.transform.position, end.transform.position);

                    if (!openSet.Contains(connectedNode))
                        openSet.Add(connectedNode);
                }
            }
        }
        return null;
    }
}
