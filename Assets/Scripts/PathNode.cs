using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathNode : MonoBehaviour
{

    public PathNode prevNode;
    public List<PathNode> connections;

    public float gScore;
    public float hScore;
    public float FScore()
    {
        return gScore + hScore;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (connections.Count > 0)
        {
            for (int i = 0; i < connections.Count; i++)
                Gizmos.DrawLine(transform.position, connections[i].transform.position);
        }
    }
}
