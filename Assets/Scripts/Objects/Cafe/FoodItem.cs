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
    public float cookTime;
    public Sprite sprite;
    public int numUpgrades;
    public MachineType machineType;
}