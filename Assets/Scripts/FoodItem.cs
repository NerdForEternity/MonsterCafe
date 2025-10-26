using UnityEngine;

[CreateAssetMenu(fileName = "New Food Item", menuName = "Cafe/Food Item")]
public class FoodItem : ScriptableObject
{
    public enum MachineType
    {
        CoffeeMachine, SodaMachine, MartiniMachine, BurgerMachine, PaniniMachine
    }
    public bool isUnlocked;
    public int price;
    public int cookTime;
    public Sprite sprite;
    public int numUpgrades;
    public MachineType machineType;

    /*public FoodItem(bool newIsUnlocked, int newPrice, int newCookTime, Sprite newSprite, int newNumUpgrades, MachineType newMachineType)
    {
        this.isUnlocked = newIsUnlocked;
        this.price = newPrice;
        this.cookTime = newCookTime;
        this.sprite = newSprite;
        this.numUpgrades = newNumUpgrades;
        this.machineType = newMachineType;
    }*/
}