using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GrimmJournal : MonoBehaviour
{
    //UI Variables
    public Image backgroundImage; // Assign the Image component from the Inspector
    public Sprite[] backgroundSprites; // Array of sprites to switch between
    public GameObject upgradesPage;
    public GameObject campaignPage;
    public GameObject unlocksPage;
    public GameObject settingsPage;
    //Upgrades
    public GameObject cookingSpeedPage;
    private GameObject cookingText;
    private GameObject cookingPriceText;
    public GameObject customerPatiencePage;
    private GameObject patienceText;
    private GameObject patiencePriceText;
    public GameObject foodPricePage;
    public GameObject priceText;
    public GameObject foodPriceText;

    private int currentSpriteIndex = 0;

    //UpgradeManager upgrades;


    private int foodIndex;

    //Audio Manager
    AudioManager audioManager;
    //UpgradeManager upgradeManager;

    public void ChangeBackground(int targetBackground)
    {
        if (backgroundImage != null && backgroundSprites != null && backgroundSprites.Length > 0)
        {
            currentSpriteIndex = targetBackground;
            backgroundImage.sprite = backgroundSprites[currentSpriteIndex];
        }
        else
        {
            Debug.LogWarning("Background Image or Sprites not assigned!");
        }
    }

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        //upgradeManager = GameObject.FindGameObjectWithTag("UpgradesManager").GetComponent<UpgradeManager>();
        CampaignPage();
    }

    public void ShowPanel(GameObject panelToShow)
    {
        // Deactivate all panels
        campaignPage.SetActive(false);
        upgradesPage.SetActive(false);
        unlocksPage.SetActive(false);
        settingsPage.SetActive(false);
        cookingSpeedPage.SetActive(false);
        customerPatiencePage.SetActive(false);
        foodPricePage.SetActive(false);


        // Activate the selected panel
        panelToShow.SetActive(true);
    }


    //Page Changes
    public void CampaignPage()
    {
        ChangeBackground(0);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(campaignPage);
        SceneManager.LoadScene("WorldMapScene", LoadSceneMode.Additive);
    }
    public void UpgradesPage()
    {
        SceneManager.UnloadSceneAsync("WorldMapScene");
        ChangeBackground(1);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);
    }
    public void UnlocksPage()
    {
        SceneManager.UnloadSceneAsync("WorldMapScene");
        ChangeBackground(2);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(unlocksPage);
    }
    public void SettingsPage()
    {
        SceneManager.UnloadSceneAsync("WorldMapScene");
        ChangeBackground(3);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(settingsPage);
    }

    //Upgrade Button Functions

    public void CookingSpeedPage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);

        cookingSpeedPage.SetActive(true);
        cookingText = cookingSpeedPage.transform.GetChild(2).gameObject;
        cookingPriceText = cookingSpeedPage.transform.GetChild(3).gameObject;

        cookingText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.cookSpeedAdd.ToString());
        if (UpgradeManager.cookSpeedAdd < 3)
            cookingPriceText.GetComponent<TMP_Text>().text = ("COST: " + (2 * (UpgradeManager.cookSpeedAdd + 1)));
        else
            cookingPriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");

    }

    public void CustomerPatiencePage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);

        customerPatiencePage.SetActive(true);
        patienceText = customerPatiencePage.transform.GetChild(2).gameObject;
        patiencePriceText = customerPatiencePage.transform.GetChild(3).gameObject;

        patienceText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.patienceAdd.ToString());
        if (UpgradeManager.patienceAdd < 3)
            patiencePriceText.GetComponent<TMP_Text>().text = ("COST: " + (2 * (UpgradeManager.patienceAdd + 1)));
        else
            patiencePriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
    }

    public void FoodPricePage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);

        foodPricePage.SetActive(true);
        priceText = foodPricePage.transform.GetChild(2).gameObject;
        foodPriceText = foodPricePage.transform.GetChild(3).gameObject;

        float upgradePrice = 2;
        if (UpgradeManager.priceAdd > 0)
        {
            for (int i = 0; i < UpgradeManager.priceAdd; i++)
                upgradePrice = upgradePrice * 1.2f;
        }
        Mathf.Round(upgradePrice);

        priceText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.priceAdd.ToString());
        foodPriceText.GetComponent<TMP_Text>().text = ("COST: " + upgradePrice);
    }

    public void upgradeCooking()
    {
        int price = 2 * (UpgradeManager.cookSpeedAdd + 1);
        if (UpgradeManager.cookSpeedAdd < 3 && UpgradeManager.totalMoney >= price)
        {
            UpgradeManager.cookSpeedAdd++;
            UpgradeManager.totalMoney -= price;
            cookingText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.cookSpeedAdd.ToString());

            if (UpgradeManager.cookSpeedAdd < 3)
                cookingPriceText.GetComponent<TMP_Text>().text = ("COST: " + 2 * (UpgradeManager.cookSpeedAdd + 1));
            else
                cookingPriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
        }
    }
    public void upgradeCustomer()
    {
        int price = 2 * ((int)UpgradeManager.patienceAdd + 1);
        if (UpgradeManager.patienceAdd < 3 && UpgradeManager.totalMoney >= price)
        {
            UpgradeManager.patienceAdd++;
            UpgradeManager.totalMoney -= price;
            patienceText.GetComponent<TMP_Text>().text = ("LVL: " + UpgradeManager.patienceAdd.ToString());

            if (UpgradeManager.patienceAdd < 3)
                patiencePriceText.GetComponent<TMP_Text>().text = ("COST: " + (2 * (UpgradeManager.patienceAdd + 1)));
            else
                patiencePriceText.GetComponent<TMP_Text>().text = ("COST: SOLD OUT");
        }
    }
    
    //remove
    public void upgradeFood()
    {
        float upgradePrice = 2f;
        if (UpgradeManager.priceAdd > 0)
        {
            for (int i = 0; i < UpgradeManager.priceAdd; i++)
                upgradePrice = upgradePrice * 1.2f;
        }
        Mathf.Round(upgradePrice);

        if (UpgradeManager.totalMoney >= (int)upgradePrice)
        {
            UpgradeManager.priceAdd++;
            UpgradeManager.totalMoney -= (int)upgradePrice;

            UpgradeManager.orderList[foodIndex].numUpgrades++;
            Debug.Log("Upgraded " + UpgradeManager.orderList[foodIndex].name + " to level " + UpgradeManager.orderList[foodIndex].numUpgrades);

        }
    }

    public void UpdatePrice()
    {
        priceText.GetComponent<TMP_Text>().text = ("LVL:" + UpgradeManager.orderList[foodIndex].numUpgrades.ToString());
        foodPriceText.GetComponent<TMP_Text>().text = ("COST: " + UpgradeManager.orderList[foodIndex].price);
        Debug.Log("do you even see this");
    }

    //Back Button Functionality
    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("GrimmJournal");
        SceneManager.UnloadSceneAsync("WorldMapScene");
    }

    public void SelectCoffee()
    {
        foodIndex = 0;
        UpdatePrice();
    }

    public void SelectSoda()
    {
        foodIndex = 1;
        UpdatePrice();
    }

    public void SelectMartini()
    {
        foodIndex = 2;
        UpdatePrice();
    }
    public void SelectBurger()
    {
        foodIndex = 3;
        UpdatePrice();
    }
    public void SelectPanini()
    {
        foodIndex = 4;
        UpdatePrice();
    }

}
