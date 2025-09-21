using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerManager : MonoBehaviour
{
    public GameObject customer;
    public GameObject door;
    public float spawnTime;
    public int numServed = 0;
public GridLayout grid;

    //this number will change, but for now the scene will only have two tables
    public List<GameObject> tables;
    private int numCustomers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CreateCustomer());
for (int i = 0; i < tables.Count; i++)
Debug.Log("Table " + i + ": " + grid.WorldToCell(tables[i].transform.position));
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    IEnumerator CreateCustomer()
    {
        //total customers cannot exceed seating
        if (numCustomers < tables.Count)
        {
            //as more customers are served, they spawn more frequently
            spawnTime = Random.Range(1f, 3f) - numServed;
            //time between spawns is 5 seconds at minimum
            if (spawnTime < 5)
                spawnTime = Random.Range(5f, 7f);

            yield return new WaitForSeconds(spawnTime);
            Instantiate(customer, door.transform);
            numCustomers++;
            StartCoroutine(CreateCustomer());
        }
    }
}
