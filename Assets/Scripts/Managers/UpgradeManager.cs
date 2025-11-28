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

        // since this script is a singleton, this is only called on game startup
        // only this script uses GetInt for the upgrades, as whenever the number of upgrades are needed...
        // ...they are accessed through here
        // if the foods are upgraded, they are updated here and SetInt is used to save the data
        coffee.numUpgrades = PlayerPrefs.GetInt("CoffeeUpgrade", 0);
        soda.numUpgrades = PlayerPrefs.GetInt("SodaUpgrade", 0);
        martini.numUpgrades = PlayerPrefs.GetInt("MartiniUpgrade", 0);
        burger.numUpgrades = PlayerPrefs.GetInt("BurgerUpgrade", 0);
        panini.numUpgrades = PlayerPrefs.GetInt("PaniniUpgrade", 0);
    }
}
