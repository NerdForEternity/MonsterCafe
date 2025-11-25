using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
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
