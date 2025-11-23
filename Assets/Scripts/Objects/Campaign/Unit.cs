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
    private Animator animator;
    public bool animationDone;
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
        if(myType == UnitType.Ghost)
            animator = this.GetComponent<Animator>();
        else
            animator = this.transform.GetChild(1).GetComponent<Animator>();
        ChangeHealth(0);
    }

    void Update()
    {
        if(!isPreview)
        {
            //if the wave has started and the unit hasn't reached the other side...
            //AND the unit isn't currently attacking
            if(unitManager.waveStarted && transform.position != targetTile.transform.position && !isAttacking)
            {
                animator.SetBool("Walking", true);
                transform.position = Vector2.MoveTowards(transform.position, targetTile.transform.position, 3 * Time.deltaTime);
            }
            else
                animator.SetBool("Walking", false);
        }
    }

    void OnTriggerEnter2D(Collider2D whatIHit)
    {
        if(!isPreview)
        {
            // check if wave has started to prevent preview units damaging existing units
            // check if unit has hit an enemy to prevent Coroutine running twice (once for unit hitting enemy, again for enemy hitting unit)
            // technically the units dont hit each other, the player units hit the enemy and then damage themselves...
            // to allow the player to always attack first
            if(whatIHit.tag == "Unit" && unitManager.waveStarted && !isEnemy)
                StartCoroutine(Attack(whatIHit.gameObject.GetComponent<Unit>()));
        }
    }

    IEnumerator Attack(Unit attackingUnit)
    {
        isAttacking = true;
        attackingUnit.isAttacking = true;

        attackingUnit.animator.SetBool("Walking", false);
        animator.SetBool("Walking", false);
        
        // -1 if resistant, -2 if neutral
        int enemyResist = -2;
        int myResist = -2;
        
        // unit = resist, attacker = neutral
        if(myType == attackingUnit.weakness) 
            myResist = -1;

        // unit = neutral, attacker = resist
        else if(weakness == attackingUnit.myType)
            enemyResist = -1;
        
        // else, both neutral

        while(health > 0 && attackingUnit.health > 0)
        {
            attackingUnit.ChangeHealth(enemyResist); // damage enemy
            ChangeHealth(myResist); // damage self
            
            attackingUnit.animator.Play("Base Layer.Attack_Anim", -1, 0f); // play enemy attack animation...
            animator.Play("Base Layer.Attack_Anim", -1, 0f); // ...and play player unit attack animation...
            
            yield return new WaitUntil(() => animationDone); // ...and wait for them to finish
            animationDone = false; // reset animation
        }

        isAttacking = false;
        attackingUnit.isAttacking = false;

        // check AFTER attack animation is done if unit is defeated
        // if enemy is defeated
        if(attackingUnit.health <= 0)
        {
            unitManager.enemies.Remove(attackingUnit.gameObject);
            Destroy(attackingUnit.gameObject);
        }
        
        // if player unit is defeated
        if(health <= 0)
        {
            unitManager.spawnedUnits.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void ChangeHealth(float healthLoss)
    {
        health+= healthLoss;     
        healthText.GetComponent<TMP_Text>().text = Mathf.Ceil(health / 2).ToString();
    }
}
