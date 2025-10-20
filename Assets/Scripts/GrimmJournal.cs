using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public GameObject customerPatiencePage;
    public GameObject foodPricePage;

    private int currentSpriteIndex = 0;


    //Audio Manager
    AudioManager audioManager;
    UpgradeManager upgradeManager;

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
        upgradeManager = GameObject.FindGameObjectWithTag("UpgradesManager").GetComponent<UpgradeManager>();
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

    }
    public void UpgradesPage()
    {
        ChangeBackground(1);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);
    }
    public void UnlocksPage()
    {
        ChangeBackground(2);
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(unlocksPage);
    }
    public void SettingsPage()
    {
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
    }

    public void CustomerPatiencePage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);
        customerPatiencePage.SetActive(true);
    }

    public void FoodPricePage()
    {
        audioManager.PlaySFX(audioManager.buttonClick);
        ShowPanel(upgradesPage);
        foodPricePage.SetActive(true);
    }

    public void upgradeCooking()
    {
        ;
    }
    public void upgradeCustomer()
    {
        ;
    }
    public void upgradeFood()
    {
        ;
    }

    //Back Button Functionality
    public void GoBack()
    {
        SceneManager.UnloadSceneAsync("GrimmJournal");
    }
}
