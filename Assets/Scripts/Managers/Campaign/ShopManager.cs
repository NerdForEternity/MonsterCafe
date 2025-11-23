using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject[] priceText;
    public GameObject[] numText;
    public GameObject moneyText;
    public GameObject UIParent;
    public GameObject roundText;
    public UnitManager UnitManager;

    void Start()
    {
        roundText.GetComponent<TMP_Text>().text = "ROUND " + UnitManager.numRounds.ToString() + "/" + UnitManager.maxRounds.ToString();
    }

    // update price of unit in shop
    public void UpdatePrice(int unitID)
    {   
       Unit unitScript = UnitManager.units[unitID].GetComponent<Unit>(); // get reference to generic unit
        if (unitScript.numBought > 0)
        {
            PlayerPrefs.SetInt("Money", ((PlayerPrefs.GetInt("Money", 0) - unitScript.price)));

            float upgradePrice = (float)unitScript.basePrice;
            for (int i = 0; i < unitScript.numBought; i++)
                upgradePrice = upgradePrice * 1.2f;

            upgradePrice = Mathf.Round(upgradePrice);
            unitScript.price = (int)upgradePrice; 
        }
        else
            unitScript.price = unitScript.basePrice;

        moneyText.GetComponent<TMP_Text>().text = (PlayerPrefs.GetInt("Money", 0)).ToString(); //update text displaying money
        priceText[unitID].GetComponent<TMP_Text>().text = ("PRICE: " + unitScript.price); //update price of shop prices
    }
    
    // update text displaying number of units in inventory
    public void UpdateAmount(int unitID)
    {
        Unit unitScript = UnitManager.units[unitID].GetComponent<Unit>();
        numText[unitID].GetComponent<TMP_Text>().text = "x" + unitScript.numInInventory.ToString();
    }

    // called when wave is started/finished
    public void ChangeUI(bool isUIOn)
    {
        UIParent.SetActive(isUIOn);
    }

    public void UpdateRounds()
    {
        roundText.GetComponent<TMP_Text>().text = "ROUND " + UnitManager.numRounds.ToString() + "/" + UnitManager.maxRounds.ToString();
    }
}
