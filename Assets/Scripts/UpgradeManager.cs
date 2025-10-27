using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //total money the player has earned
    public static int totalMoney = 0;
    //food takes 0.5 seconds less to prepare, max of 3 upgrades
    public static int cookSpeedAdd = 0;
    //customers wait for 1 second longer, max of 3 upgrades
    public static float patienceAdd = 0f;
    //increases price of everything by 20%, no cap
    //NOTE: replace with priceAdd for individual food items
    public static int priceAdd = 0;
    
    public Sprite coffeeSprite;
    public Sprite sodaSprite;
    public Sprite martiniSprite;
    public Sprite burgerSprite;
    public Sprite paniniSprite;

    public static FoodItem coffee;
    public static FoodItem soda;
    public static FoodItem martini;
    public static FoodItem burger;
    public static FoodItem panini;

    public static FoodItem[] orderList;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        coffee = new FoodItem(true, 2, 3, coffeeSprite, 0, "coffee");
        soda = new FoodItem(true, 4, 4, sodaSprite, 0, "soda");
        martini = new FoodItem(true, 6, 5, martiniSprite, 0, "martini");
        burger = new FoodItem(true, 8, 6, burgerSprite, 0, "burger");
        panini = new FoodItem(true, 10, 7, paniniSprite, 0, "panini");

        orderList = new FoodItem[]{coffee, soda, martini, burger, panini};
    }
}
