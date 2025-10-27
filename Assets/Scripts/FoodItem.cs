using UnityEngine;

public class FoodItem
{
    public bool isUnlocked;
    public int price;
    public int cookTime;
    public Sprite sprite;
    public int numUpgrades;
    public string name;

    public FoodItem(bool newIsUnlocked, int newPrice, int newCookTime, Sprite newSprite, int newNumUpgrades, string newName)
    {
        this.isUnlocked = newIsUnlocked;
        this.price = newPrice;
        this.cookTime = newCookTime;
        this.sprite = newSprite;
        this.numUpgrades = newNumUpgrades;
        this.name = newName;
    }
}