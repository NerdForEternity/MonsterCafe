using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    // objects
    public List<GameObject> customer;
    public GameObject door;
    public List<Machine> machines;
    public List<Chair> chairs;
    private PathNode closestNode; 
    
    // variables / flags
    public float spawnTime;
    public int numServed = 0;
    public bool idle;
    public int numCustomers;

    // managers / sprites
    public DecorShop decorShop;

    void Start()
    {
        idle = false;

        foreach (Chair n in FindObjectsByType<Chair>(FindObjectsSortMode.None))
            chairs.Add(n);

        foreach (Machine n in FindObjectsByType<Machine>(FindObjectsSortMode.None))
        {
            machines.Add(n);
            if(!n.itemType.isUnlocked)
                n.gameObject.SetActive(false);
        }

        Time.timeScale = PlayerPrefs.GetFloat("GameSpeed", 1f);
        decorShop.SwapTiles(0, PlayerPrefs.GetInt("Decor", 0));
        StartCoroutine(CreateCustomer());
    }

    IEnumerator CreateCustomer()
    {
        //total customers cannot exceed seating
        yield return new WaitUntil(() => numCustomers < chairs.Count);

        //as more customers are served, they spawn more frequently
        spawnTime = Random.Range(10f, 9f) - (numServed * 0.25f);
        //time between spawns is 2 seconds at minimum
        if (spawnTime < 2)
            spawnTime = Random.Range(2.0f, 2.5f);
        yield return new WaitForSeconds(spawnTime);
        
        //get customer spawn point
        PathNode doorNode = GetClosestNode();
        //randomize customer appearance
        int customerType = Random.Range(0, customer.Count - 1);
        //create customer
        GameObject newCustomer = Instantiate(customer[customerType], doorNode.transform.position, doorNode.transform.rotation);

        //pass references to new customer
        Customer scriptRef = newCustomer.GetComponent<Customer>();
        scriptRef.startNode = doorNode;
        scriptRef.manager = this.GetComponent<CustomerManager>();

        numCustomers++;

        StartCoroutine(CreateCustomer());
    }

    public PathNode GetClosestNode()
    {
        //function is called before spawning customer, no need to update when moved
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
