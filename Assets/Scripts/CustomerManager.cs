using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject customer;
    public GameObject door;
    public float spawnTime;
    public int numServed = 0;

    //this number will change, but for now the scene will only have three chairs
    public List<Chair> chairs;
    private int numCustomers;
    private PathNode closestNode;

    void Start()
    {
        List<Chair> chairs = new List<Chair>();

        foreach (Chair n in FindObjectsByType<Chair>(FindObjectsSortMode.None))
            chairs.Add(n);

        StartCoroutine(CreateCustomer());
    }

    IEnumerator CreateCustomer()
    {
        //total customers cannot exceed seating
        if (numCustomers < chairs.Count)
        {
            //as more customers are served, they spawn more frequently
            spawnTime = Random.Range(1f, 3f) - numServed;
            //time between spawns is 5 seconds at minimum
            if (spawnTime < 5)
                spawnTime = Random.Range(5f, 7f);

            yield return new WaitForSeconds(spawnTime);
            //create customer
            PathNode doorNode = GetClosestNode();
            GameObject newCustomer = Instantiate(customer, doorNode.transform);

            //pass reference to chairs to new customer
            Customer scriptRef = newCustomer.GetComponent<Customer>();
            scriptRef.chairs = chairs;
            scriptRef.startNode = doorNode;
            numCustomers++;
            StartCoroutine(CreateCustomer());
        }
    }
    
    public PathNode GetClosestNode()
    {
        //note: write code to update position if moved by player
        Vector2 doorPos = door.transform.position;
        float minDistance = Mathf.Infinity;
        foreach (PathNode n in FindObjectsByType<PathNode>(FindObjectsSortMode.None))
        {
            Vector2 currentNodePos = n.transform.position;

            float dist = Vector2.Distance(n.transform.position, doorPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestNode = n;
            }
        }

        return closestNode;
    }
}
