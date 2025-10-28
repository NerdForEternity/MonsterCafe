using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    //total money the player has earned
    public static int totalMoney = 0;
    //food takes 0.5 seconds less to prepare, max of 3 upgrades
    public static int cookSpeedAdd = 0;
    //customers wait for 1 second longer, max of 3 upgrades
    public static float patienceAdd = 0f;

    public FoodItem coffee;
    public FoodItem soda;
    public FoodItem martini;
    public FoodItem burger;
    public FoodItem panini;

    public static FoodItem[] orderList;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        orderList = new FoodItem[]{coffee, soda, martini, burger, panini};
    }
}
