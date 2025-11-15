using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public enum UnitType
    {
        Ghost, Vampire, Werewolf
    }
    public int health;
    public int numInInventory; // number the player can place down; changes as units are bought and placed
    public int numBought; // number of this unit bought; determines price and only resets after each round
    public int basePrice; // base price of this unit; used to calculate final price
    public int price; // actual price of unit
    public GameObject healthText;
    public UnitType myType;
    public UnitType weakness;

    void Start()
    {
        healthText = this.transform.GetChild(0).GetChild(0).gameObject;
        ChangeHealth(0);
    }

    /*void Attack(Unit attackingUnit)
    {
        possible situations:
        - this unit is strong against attacker: attacker is defeated, next hit deletes this attacker
        - this unit is netural against attacker: both are defeated
        - this unit is weak to attacker
        
        // this unit is strong against attacker
        if(attackingUnit.weakness == myType)
            ChangeHealth(-1);
        
        // this unit is neutral or weak to attacker
        else
            ChangeHealth(-2);
    }*/

    public void ChangeHealth(int healthLoss)
    {
        health+= healthLoss;     
        healthText.GetComponent<TMP_Text>().text = Mathf.Ceil(health / 2).ToString();

        if(health == 0)
            Destroy(this.gameObject);
    }
}
