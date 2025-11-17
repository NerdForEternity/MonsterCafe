using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Unit : MonoBehaviour
{
    public enum UnitType
    {
        Ghost, Vampire, Werewolf
    }
    
    public UnitManager unitManager;
    public float health;
    public GameObject healthText; // text displaying remaining health
    public GameObject myTile; // tile the unit spawned at
    public GameObject targetTile; // tile containing the enemies this unit will face
    private bool isAttacking; // checks if this unit is attacking an enemy
    public bool isEnemy;
    public UnitType myType;
    public UnitType weakness;

    // variables exclusive to player units
    public int numInInventory; // number the player can place down; changes as units are bought and placed
    public int numBought; // number of this unit bought; determines price and only resets after each round
    public int basePrice; // base price of this unit; used to calculate final price
    public int price; // actual price of unit
    public bool isPreview; // checks if this is a preview or comfirmed unit

    void Start()
    {
        healthText = this.transform.GetChild(0).GetChild(0).gameObject;
        ChangeHealth(0);
    }

    void Update()
    {
        if(!isPreview)
        {
            //if the wave has started and the unit hasn't reached the other side...
            //AND the unit isn't currently attacking
            if(unitManager.waveStarted && transform.position != targetTile.transform.position && !isAttacking)
                transform.position = Vector2.MoveTowards(transform.position, targetTile.transform.position, 3 * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if(!isPreview)
        {
            // check if wave has started to prevent preview units damaging existing units
            if(whatIHit.tag == "Unit" && unitManager.waveStarted)
                StartCoroutine(Attack(whatIHit.gameObject.GetComponent<Unit>()));
        }
    }

    IEnumerator Attack(Unit attackingUnit)
    {
        isAttacking = true;
        
        // this unit is strong against attacker
        if(attackingUnit.weakness == myType)
            ChangeHealth(-1);
        
        // this unit is neutral or weak to attacker
        else
            ChangeHealth(-2);
        
        yield return new WaitForSeconds(2);
        isAttacking = false;
    }

    public void ChangeHealth(int healthLoss)
    {
        health+= healthLoss;     
        healthText.GetComponent<TMP_Text>().text = Mathf.Ceil(health / 2).ToString();

        if(health <= 0)
        {
            // remove from respective list
            if(isEnemy)
                unitManager.enemies.Remove(this.gameObject);
            else
                unitManager.spawnedUnits.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
